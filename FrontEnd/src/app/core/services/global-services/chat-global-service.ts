import { Injectable, OnDestroy, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { IMessageDTO } from '../../DTO/message-dto';
import { SendMessageDTO as ISendMessageDTO } from '../../DTO/send-message-dto';
import { ApiConstrants } from '../../constants/api-constants';

@Injectable({ providedIn: 'root' })
export class ChatHubService implements OnDestroy {
  // observers
  readonly newMessage$ = new Subject<IMessageDTO>();
  readonly typingUserId$ = new Subject<void>();
  readonly stoppedTypingUserId$ = new Subject<void>();
  
  // fields
  private _hubConnection!: signalR.HubConnection;
  private readonly _hubUrl: string = ApiConstrants.serverUrl + '/hubs/chat';

  startConnection = async () : Promise<boolean> => {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this._hubUrl, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    this.registerHandlers();

    try{
      await this._hubConnection.start()

      this.joinGroup()

      return true;
    }
    catch(ex){
      // toDo: show error message
      return false;
    }
  };

  private registerHandlers = () => {
    this._hubConnection.on('ReceiveMessageAsync', (msg: IMessageDTO) => {
      this.newMessage$.next(msg);
    });

    this._hubConnection.on('UserIsTypingAsync', () => {
      this.typingUserId$.next();
    });

    this._hubConnection.on('UserStoppedTypingAsync', () => {
      this.stoppedTypingUserId$.next();
    });
  };

  private joinGroup = () => {
    return this._hubConnection.invoke('JoinChatGroup');
  };

  sendMessage = (dto: ISendMessageDTO) => {
    return this._hubConnection.invoke('SendMessage', dto);
  };

  notifyTyping = () => {
    return this._hubConnection.invoke('NotifyTyping');
  };

  notifyStoppedTyping = () => {
    return this._hubConnection.invoke('NotifyStoppedTyping');
  };

  stopConnection = () => {
    return this._hubConnection?.stop();
  };

  ngOnDestroy(): void {
    this.stopConnection();
  }
}
