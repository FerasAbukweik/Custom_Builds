import { inject, Injectable, signal } from '@angular/core';
import { IMessageDTO } from '../../DTO/message-dto';
import { MessagesApiService } from '../api-services/message-api-service';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { MessagesSignalRService } from './messaegs-signalR-service';

@Injectable({ providedIn: 'root' })
export class MessagesService {
  // injections
  private readonly _messageApiService = inject(MessagesApiService);
  private readonly _messagesSignalRService = inject(MessagesSignalRService);

  // signals
  private _isLoading = signal<boolean>(false);
  private _messages = signal<IMessageDTO[]>([]);
  private _isTyping = signal<boolean>(false);
  private _isSignalRConnected = signal<boolean>(false);

  // fields

  // private
  private _lazyData: ILazyDTO = {
    sectionSize: 10,
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

  // constructor
  constructor() {
    this.handleReceiveMessage();
  }

  // methods

  // add message
  private addMessage = (msg: IMessageDTO) => {
    this._messages.update((curr) =>
      [...curr, msg].sort((a, b) => (a.createdAt > b.createdAt ? 1 : -1)),
    );
  };

  // lazy load items
  public lazyGetMessages() {
    if (this._isLoading() || !this._isMoreDataAvaiable) return;
    this._isLoading.set(true);

    this._messageApiService.lazyGetMessages(this._lazyData).subscribe({
      next: (res) => {
        this._messages.update((curr) =>
          [...curr, ...res].sort((a, b) => (a.createdAt > b.createdAt ? 1 : -1)),
        );

        this._lazyData.taken += res.length;
        this._isMoreDataAvaiable = res.length > 0;
        this._isLoading.set(false);
      },
      error: () => {
        this._isLoading.set(false);
      },
    });
  }

  private handleReceiveMessage = () => {
    this._messagesSignalRService.receiveMessage$.subscribe({
      next: (msg) => {
        this.addMessage(msg as IMessageDTO);
        this._lazyData.taken++;
      },
    });
  };
}
