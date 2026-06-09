import { DestroyRef, inject, Injectable, signal } from '@angular/core';
import { IMessageDTO } from '../../../../../core/DTO/message-dto';
import { MessagesService } from '../../../../../core/services/api-services/message-service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ILazyLoadingDTO } from '../../../../../core/DTO/lazy-loading-dto';
import { firstValueFrom } from 'rxjs';
import { ChatHubService } from '../../../../../core/services/global-services/chat-global-service';
import { MessageTypeEnum } from '../../../../../core/enums/message-type-enum';
import { SendMessageDTO } from '../../../../../core/DTO/send-message-dto';

@Injectable({ providedIn: 'root' })
export class SupportService {
  // injections
  private readonly _messageService = inject(MessagesService);
  private readonly _chatService = inject(ChatHubService);
  private readonly _destroyRef = inject(DestroyRef);

  // signals
  private _isLoading = signal<boolean>(false);
  private _messages = signal<IMessageDTO[]>([]);
  private _isTyping = signal<boolean>(false);
  private _isSignalRConnected = signal<boolean>(false);

  // fields

  // private
  private _requestMessagesData: ILazyLoadingDTO = {
    ElementsPerSection: 10,
    taken: 0,
  };
  private _isMoreDataAvaiable: boolean = true;

  // getters
  get getIsLoading() {
    return this._isLoading.asReadonly();
  }

  get getMessages() {
    return this._messages.asReadonly();
  }

  get getIsTyping() {
    return this._isTyping.asReadonly();
  }

  get getIsSignalRConnected() {
    return this._isSignalRConnected.asReadonly();
  }

  // methods

  // init
  async init() {
    // start connection with the hub
    let isSignalRConnected = await this._chatService.startConnection();
    if (!isSignalRConnected) {
      // toDo: show error message
    }

    this._isSignalRConnected.set(isSignalRConnected);

    // get inital messages
    this.lazyGetMessages();

    this.handleReceiveMessage();
    this._handleUserTyping();
    this._handleStoppedTyping();
  }

  // add tokens
  addToTaken(toAdd: number) {
    this._requestMessagesData.taken += toAdd;
  }

  // add message
  public addMessage = (msg: IMessageDTO) => {
    this._messages.update((curr) =>
      [...curr, msg].sort((a, b) => (a.createdAt > b.createdAt ? 1 : -1)),
    );
  };

  // lazy load items
  public lazyGetMessages = async (): Promise<boolean> => {
    if (this._isLoading() || !this._isMoreDataAvaiable) return false;
    this._isLoading.set(true);

    try {
      const res = await firstValueFrom(
        this._messageService
          .lazyGetMessages(this._requestMessagesData)
          .pipe(takeUntilDestroyed(this._destroyRef)),
      );

      this._messages.update((curr) =>
        [...curr, ...res].sort((a, b) => (a.createdAt > b.createdAt ? 1 : -1)),
      );

      this._requestMessagesData.taken += res.length;
      this._isMoreDataAvaiable = res.length > 0;
      this._isLoading.set(false);

      return true;
    } catch (err: any) {
      if (err.status === 404) this._isMoreDataAvaiable = false;
      this._isLoading.set(false);

      return false;
    }
  };

  // ---------------------------------- signalR related ----------------------------------

  // handle receive message
  private handleReceiveMessage = () => {
    this._chatService.newMessage$.pipe(takeUntilDestroyed(this._destroyRef)).subscribe({
      next: (msg) => {
        this.addMessage(msg as IMessageDTO);
        this.addToTaken(1);
      },
    });
  };

  // handle user is typing
  private _handleUserTyping = () => {
    this._chatService.typingUserId$.pipe(takeUntilDestroyed(this._destroyRef)).subscribe({
      next: () => {
        this._isTyping.set(true);
      },
    });
  };

  // handle stpped typing
  private _handleStoppedTyping = () => {
    this._chatService.stoppedTypingUserId$.pipe(takeUntilDestroyed(this._destroyRef)).subscribe({
      next: () => {
        this._isTyping.set(false);
      },
    });
  };

  // manage is typing
  handleIsTyping = (isInputEmpty: boolean) => {
    if (isInputEmpty) this._chatService.notifyStoppedTyping();
    else this._chatService.notifyTyping();
  };

  // handel send message
  handleSendMessage = (input: string) => {
    const toSendMessage: SendMessageDTO = {
      messageType: MessageTypeEnum.text,
      content: input,
    };

    this._chatService.sendMessage(toSendMessage);
  };

  // ---------------------------------- signalR related ----------------------------------
}
