import { CommonModule } from '@angular/common';
import {
  afterNextRender,
  Component,
  DestroyRef,
  ElementRef,
  inject,
  OnInit,
  signal,
  viewChild,
} from '@angular/core';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
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
  messages = this._supportService.getMessages;
  isLoading = this._supportService.getIsLoading;
  currUserId = this._supportService.getCurrUserId;
  messageInput = signal<string>('');
  isTyping = signal<boolean>(false);
  isSignalRConnected = signal<boolean>(false);

  // viewChild
  myScrollContainer = viewChild.required<ElementRef<HTMLDivElement>>('myScrollContainer');

  // fields

  // private

  // public
  quickActions = quickActions;


  constructor() {
  const sub = toObservable(this.isLoading).subscribe({
    next: (isLoading) => {
      if (!isLoading && this.currUserId()) {
        this._scrollToBottom();
        sub.unsubscribe();
      }
    }
  });
}


  // methods
  async ngOnInit() {
    // get important inital data
    if(!this.currUserId() || !this._supportService.getChatGroupId){
      const getInitDataRes = await this._supportService.getInitialData();
      if (!getInitDataRes) {
        // toDo: show error message
        return;
      }

      // set curr user id
      this._supportService.setCurrUserId(getInitDataRes.userId);

      // set chatGroup id so we can send it with the add message request
      this._supportService.setChatGroupId(getInitDataRes.chatGroupId);
    }

    // in case we already had messages in the service
    this._scrollToBottom();

    // start connection with the hub
    let isSignalRConnected = await this._chatService.startConnection(this._supportService.getChatGroupId);
    if (!isSignalRConnected) {
      // toDo: show error message
    }

    this.isSignalRConnected.set(isSignalRConnected);

    // get inital messages
    this._supportService.lazyGetMessages();


    this.handleReceiveMessage();
    this._handleUserTyping();
    this._handleStoppedTyping();
  }

  // lazy load messages
  lazyLoadMessages = this._supportService.lazyGetMessages;

  // scroll to bottom
  private _scrollToBottom = (delay: number = 0) => {
    setTimeout(() => {
      this.myScrollContainer().nativeElement.scrollTo(
        0,
        this.myScrollContainer().nativeElement.scrollHeight,
      );
    } , delay);
  };

  private _scrollBy = (scrollVal: number , delay: number = 100) => {
    setTimeout(() => {
      this.myScrollContainer().nativeElement.scrollBy({
        top: scrollVal,
        behavior: 'smooth',
      });
    } , delay);
  };

  // ---------------------------------- signalR related ----------------------------------

  // handle receive message
  private handleReceiveMessage = () => {
    this._chatService.newMessage$.pipe(takeUntilDestroyed(this._destroyRef)).subscribe({
      next: (msg) => {
        this._supportService.addMessage(msg as IMessageDTO);
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
    if (this.messageInput()) this._chatService.notifyTyping(this._supportService.getChatGroupId);
    else this._chatService.notifyStoppedTyping(this._supportService.getChatGroupId);
  };

  // handel send message
  handleSendMessage = (input: string) => {
    const toSendMessage: ISendMessageDTO = {
      messageType: MessageTypeEnum.text,
      content: input,
      ChatGroupId: this._supportService.getChatGroupId,
    };

    this._chatService.sendMessage(toSendMessage);
    this.messageInput.set('');
  };

  // ---------------------------------- signalR related ----------------------------------
}