import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { IMessageDTO } from '../../DTO/message-dto';
import { Urls } from '../../constants/urls';

@Injectable({ providedIn: 'root' })
export class MessagesSignalRService {
  // observers
  readonly receiveMessage$ = new Subject<IMessageDTO>();
  readonly someoneIsTyping$ = new Subject<string>();
  readonly noOneIsTyping$ = new Subject<string>();

  // private
  private _hubConnection!: signalR.HubConnection;
  private readonly _hubUrl: string = Urls.baseUrl + '/hubs/chat';

  // signals
  private _isConntected = signal<boolean>(false);

  // getters

  get isConnected() {
    return this._isConntected.asReadonly();
  }

  startConnection = async (): Promise<boolean> => {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this._hubUrl, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    try {
      await this._hubConnection.start();
      this.registerHandlers();
      this._isConntected.set(true);

      return true;
    } catch (ex) {
      // toDo: show error message
      this._isConntected.set(false);
      return false;
    }
  };

  private registerHandlers = () => {
    this._hubConnection.on('ReceiveMessageAsync', (msg: IMessageDTO) => {
      this.receiveMessage$.next(msg);
    });

    this._hubConnection.on('UserIsTypingAsync', (chatGroupId: string) => {
      this.someoneIsTyping$.next(chatGroupId);
    });

    this._hubConnection.on('UserStoppedTypingAsync', (chatGroupId: string) => {
      this.noOneIsTyping$.next(chatGroupId);
    });
  };

  sendMessage = (content: string) => {
    return this._hubConnection.invoke('SendMessage', content);
  };

  notifyTyping = () => {
    return this._hubConnection.invoke('NotifyTyping');
  };

  notifyStoppedTyping = () => {
    return this._hubConnection.invoke('NotifyStoppedTyping');
  };

  stopConnection = () => {
    try {
      this._hubConnection?.stop();
      this._isConntected.set(false);
      return true;
    } catch {
      return false;
    }
  };
}
