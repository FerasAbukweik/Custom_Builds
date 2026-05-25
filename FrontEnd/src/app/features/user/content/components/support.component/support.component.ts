import { CommonModule } from '@angular/common';
import {
  Component,
  DestroyRef,
  ElementRef,
  inject,
  OnInit,
  signal,
  viewChild,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { SupportService } from './support.service';
import { ChatHubService } from '../../../../../core/services/global-services/chat-global-service';
import { IMessageDTO } from '../../../../../core/DTO/message-dto';
import { SendMessageDTO as ISendMessageDTO } from '../../../../../core/DTO/send-message-dto';
import { MessageTypeEnum } from '../../../../../core/enums/message-type-enum';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component/loading.component';
import { IsVisableDirective } from '../../../../../shared/directives/is-visable.directive';

const quickActions = [
  'Where is my order?',
  'Customization help',
  'Payment issue',
  'Firmware update',
  'Warranty claim',
];

@Component({
  selector: 'app-support',
  imports: [CommonModule, FormsModule, LoadingComponent, IsVisableDirective],
  standalone: true,
  templateUrl: './support.component.html',
  host: {
    class:
      'flex flex-col h-full bg-dark-deep-blue rounded-xl overflow-hidden border border-dark-blue-gray shadow-2xl max-w-450 mx-auto',
  },
})
export class SupportComponent implements OnInit {
  // injections
  private readonly _supportService = inject(SupportService);
  private readonly _chatService = inject(ChatHubService);
  private readonly _destroyRef = inject(DestroyRef);

  // signals
  messages = signal<IMessageDTO[]>([]);
  isLoading = this._supportService.getIsLoading;
  currUserId = signal<string>('');
  messageInput = signal<string>('');
  isTyping = signal<boolean>(false);
  isSignalRConnected = signal<boolean>(false);

  // viewChild
  myScrollContainer = viewChild.required<ElementRef<HTMLDivElement>>('myScrollContainer');

  // fields

  // priavte
  private _chatGroupId!: string;

  // public
  quickActions = quickActions;

  // methods
  async ngOnInit() {
    // get important inital data
    const getInitDataRes = await this._supportService.getInitialData();
    if (!getInitDataRes) {
      // toDo: show error message
      return;
    }

    // start connection with the hub
    let isSignalRConnected = await this._chatService.startConnection(getInitDataRes.chatGroupId);
    if (!isSignalRConnected) {
      // toDo: show error message
    }

    this.isSignalRConnected.set(isSignalRConnected);

    // get inital messages
    this._supportService.lazyGetMessages();

    // set curr user id
    this.currUserId.set(getInitDataRes.userId);

    // set chatGroup id so we can send it with the add message request
    this._chatGroupId = getInitDataRes.chatGroupId;

    this.handleReceiveMessage();
    this._handleUserTyping();
    this._handleStoppedTyping();

    this._handleLazyGetMessages();
  }

  // lazy load messages
  lazyLoadMessages = this._supportService.lazyGetMessages;

  // scroll to bottom
  private _scrollToBottom = () => {
    setTimeout(() => {
      this.myScrollContainer().nativeElement.scrollTo(
        0,
        this.myScrollContainer().nativeElement.scrollHeight,
      );
    }, 100);
  };

  private _scrollBy = (scrollVal: number) => {
    setTimeout(() => {
      this.myScrollContainer().nativeElement.scrollBy({
        top: scrollVal,
        behavior: 'smooth'
      })
    }, 100);
  }

  // ---------------------------------- signalR related ----------------------------------

  // handle receive message
  private handleReceiveMessage = () => {
    this._chatService.newMessage$.pipe(takeUntilDestroyed(this._destroyRef)).subscribe({
      next: (msg) => {
        this.messages.update((curr) =>
          [...curr, msg as IMessageDTO].sort((a, b) => (a.createdAt > b.createdAt ? 1 : -1)),
        );

        this._scrollToBottom();
      },
    });
  };

  // handle user is typing
  private _handleUserTyping = () => {
    this._chatService.typingUserId$.pipe(takeUntilDestroyed(this._destroyRef)).subscribe({
      next: (res) => {
        const newTypingId = res as string;

        if (this.currUserId() !== newTypingId) this.isTyping.set(true);
      },
    });
  };

  // handle stpped typing
  private _handleStoppedTyping = () => {
    this._chatService.stoppedTypingUserId$.pipe(takeUntilDestroyed(this._destroyRef)).subscribe({
      next: (res) => {
        const newStoppedTypingId = res as string;

        if (this.currUserId() !== newStoppedTypingId) this.isTyping.set(false);
      },
    });
  };

  // manage is typing
  manageIsTyping = () => {
    if (this.messageInput()) this._chatService.notifyTyping(this._chatGroupId);
    else this._chatService.notifyStoppedTyping(this._chatGroupId);
  };

  // handel send message
  handleSendMessage = (input: string) => {
    const toSendMessage: ISendMessageDTO = {
      messageType: MessageTypeEnum.text,
      content: input,
      ChatGroupId: this._chatGroupId,
    };

    this._chatService.sendMessage(toSendMessage);
    this.messageInput.set('');
  };

  // ---------------------------------- signalR related ----------------------------------

  // handle lazy get messages
  private _handleLazyGetMessages = () => {
    this._supportService.newMesssages$.pipe(takeUntilDestroyed(this._destroyRef)).subscribe({
      next: (data) => {
        const newMessages = data as IMessageDTO[];

        let idScrollToBottom: boolean = false;
        if(!this.messages().length) idScrollToBottom = true;

        this.messages.update((curr) =>
          [...curr, ...newMessages].sort((a, b) => (a.createdAt > b.createdAt ? 1 : -1)),
        );

        if(idScrollToBottom) this._scrollToBottom();
        else this._scrollBy(0);
      },
    });
  };
}
