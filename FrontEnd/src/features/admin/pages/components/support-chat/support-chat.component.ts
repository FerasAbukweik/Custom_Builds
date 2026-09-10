import { Component, effect, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { LiveChattingComponent } from '../../../../../shared/components/live-chatting/live-chatting.component';
import { ChatGroupService } from './chat-group-service';
import { DatePipe } from '@angular/common';
import { IsVisableDirective } from 'src/shared/directives/is-visable.directive';
import { LoadingComponent } from 'src/shared/components/loading/loading.component';
import { AdminMessagesService } from './admin-messages-service';
import { MessagesSignalRService } from 'src/core/services/client-services/messaegs-signalR-service';

@Component({
  selector: 'app-support-chat.component',
  imports: [DatePipe, IsVisableDirective, LoadingComponent, LiveChattingComponent],
  templateUrl: './support-chat.component.html',
})
export class SupportChatComponent implements OnInit, OnDestroy {
  // DI
  protected readonly chatGroupService = inject(ChatGroupService);
  protected readonly adminMessagesService = inject(AdminMessagesService);
  protected readonly messagesSignalRService = inject(MessagesSignalRService);

  // signals
  protected selectedChatGroupId = signal<string>('');
  protected showMessagesComponent = signal<boolean>(false);

  // private
  private oldChatGroupId: string = '';

  // constructor
  constructor() {
    effect(() => {
      const newChatGroupId = this.selectedChatGroupId();

      // if signalR is not connected stop
      if (!this.messagesSignalRService.isConnected() || !newChatGroupId) return;

      // refresh the messages component
      this.showMessagesComponent.set(false);
      setTimeout(() => {
        this.showMessagesComponent.set(true);
      }, 0);

      this.switchSignalRGroup(this.oldChatGroupId, newChatGroupId);

      this.oldChatGroupId = newChatGroupId;
    });
  }

  private async switchSignalRGroup(oldId: string | null, newId: string) {
    try {
      // 1. Leave the old group first if it exists
      if (oldId) {
        await this.messagesSignalRService.LeaveChatGroup(oldId);
      }
      // 2. Join the new group
      await this.messagesSignalRService.JoinChatGroup(newId);
    } catch (err) {
      console.error('Failed to switch SignalR chat groups:', err);
    }
  }

  // methods
  ngOnInit(): void {
    this.messagesSignalRService.startConnection();
  }

  ngOnDestroy(): void {
    this.messagesSignalRService.stopConnection();
  }

  handleIsTyping(isTyping: boolean) {
    if (isTyping) this.messagesSignalRService.notifyTyping(this.selectedChatGroupId());
    else this.messagesSignalRService.notifyStoppedTyping();
  }
}
