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
  readonly typingUserId$ = new Subject<string>();
  readonly stoppedTypingUserId$ = new Subject<string>();
  
  // fields
  private _hubConnection!: signalR.HubConnection;
  private readonly _hubUrl: string = ApiConstrants.serverUrl + '/hubs/chat';

  startConnection = async (chatGroupId: string) : Promise<boolean> => {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this._hubUrl, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    this.registerHandlers();

    try{
      await this._hubConnection.start()

      this.joinGroup(chatGroupId)

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

    this._hubConnection.on('UserIsTypingAsync', (senderId: string) => {
      this.typingUserId$.next(senderId);
    });

    this._hubConnection.on('UserStoppedTypingAsync', (senderId: string) => {
      this.stoppedTypingUserId$.next(senderId);
    });
  };

  private joinGroup = (chatGroupId: string) => {
    return this._hubConnection.invoke('JoinChatGroup', chatGroupId);
  };

  sendMessage = (dto: ISendMessageDTO) => {
    return this._hubConnection.invoke('SendMessage', dto);
  };

  notifyTyping = (groupId: string) => {
    return this._hubConnection.invoke('NotifyTyping', groupId);
  };

  notifyStoppedTyping = (groupId: string) => {
    return this._hubConnection.invoke('NotifyStoppedTyping', groupId);
  };

  stopConnection = () => {
    return this._hubConnection?.stop();
  };

  ngOnDestroy(): void {
    this.stopConnection();
  }
}
