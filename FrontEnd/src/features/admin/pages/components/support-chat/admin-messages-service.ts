import { inject, Injectable, signal } from '@angular/core';
import { ILazyDTO } from 'src/core/DTO/lazy-dto';
import { IMessageDTO } from 'src/core/DTO/message-dto';
import { AdminApiService } from 'src/core/services/api-services/admin-api-service';
import { MessagesSignalRService } from 'src/core/services/client-services/messaegs-signalR-service';

@Injectable({ providedIn: 'root' })
export class AdminMessagesService {
  // injections
  private readonly _adminApiService = inject(AdminApiService);
  private readonly _messagesSignalRService = inject(MessagesSignalRService);

  // signals
  private _isLoading = signal<Record<string, boolean>>({});
  private _messages = signal<Record<string, IMessageDTO[]>>({});
  private _isTyping = signal<Record<string, boolean>>({});

  // fields
  private readonly _defaultSectionSize = 10;

  // private
  private _lazyData: Record<string, ILazyDTO> = {};
  private _isMoreDataAvailable: Record<string, boolean> = {};

  // getters
  get isLoading() {
    return this._isLoading.asReadonly();
  }

  get messages() {
    return this._messages.asReadonly();
  }

  get isTyping() {
    return this._isTyping.asReadonly();
  }

  // constructor
  constructor() {
    this.handleReceiveMessage();
  }

  // methods

  // Initialize group state if it doesn't exist
  private initGroupStateIfNotExists(groupId: string) {
    if (!this._lazyData[groupId]) {
      this._lazyData[groupId] = {
        sectionSize: this._defaultSectionSize,
        taken: 0,
      };
      this._isMoreDataAvailable[groupId] = true;
    }
  }

  // add message
  private addMessage = (msg: IMessageDTO) => {
    this._messages.update((curr) => {
      const groupMessages = curr[msg.chatGroupId] || [];
      return {
        ...curr,
        [msg.chatGroupId]: [...groupMessages, msg].sort((a, b) =>
          a.createdAt > b.createdAt ? 1 : -1,
        ),
      };
    });
  };

  // lazy load items
  public lazyGetMessages(groupId: string) {
    this.initGroupStateIfNotExists(groupId);

    const isGroupLoading = this._isLoading()[groupId];
    if (isGroupLoading || !this._isMoreDataAvailable[groupId] || !groupId) return;

    this._isLoading.update((curr) => ({ ...curr, [groupId]: true }));

    // Note: ensure your lazyGetMessages API method accepts the groupId as a parameter
    this._adminApiService.lazyGetMessages(groupId, this._lazyData[groupId]).subscribe({
      next: (res) => {
        this._messages.update((curr) => {
          const groupMessages = curr[groupId] || [];
          return {
            ...curr,
            [groupId]: [...groupMessages, ...res].sort((a, b) =>
              a.createdAt > b.createdAt ? 1 : -1,
            ),
          };
        });

        this._lazyData[groupId].taken += res.length;
        this._isMoreDataAvailable[groupId] = res.length > 0;

        this._isLoading.update((curr) => ({ ...curr, [groupId]: false }));
      },
      error: () => {
        this._isLoading.update((curr) => ({ ...curr, [groupId]: false }));
      },
    });
  }

  private handleReceiveMessage = () => {
    this._messagesSignalRService.receiveMessage$.subscribe({
      next: (msg) => {
        this.initGroupStateIfNotExists(msg.chatGroupId);
        this.addMessage(msg);
        this._lazyData[msg.chatGroupId].taken++;
      },
    });
  };
}
