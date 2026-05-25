import { DestroyRef, inject, Injectable, signal } from '@angular/core';
import { IMessageDTO } from '../../../../../core/DTO/message-dto';
import { MessagesService } from '../../../../../core/services/api-services/message-service';
import { ILazyLoadMessagesDTO } from '../../../../../core/DTO/lazy-load-messages-dto';
import { ChatGroupService } from '../../../../../core/services/api-services/chat-group-service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IInitChatGroupDataDTO } from '../../../../../core/DTO/init-chat-group-data-dto';
import { firstValueFrom } from 'rxjs';
import { Subject } from 'rxjs';

@Injectable()
export class SupportService {
  // injections
  private readonly _messageService = inject(MessagesService);
  private readonly _chatGroupService = inject(ChatGroupService);
  private readonly _destroyRef = inject(DestroyRef);

  // observers
  readonly newMesssages$ = new Subject<IMessageDTO[]>();

  // signals
  private _isLoading = signal<boolean>(false);

  //fields

  // private
  private _requestMessagesData: ILazyLoadMessagesDTO = {
    ElementsPerSection: 10,
    taken: 0,
    chatGroupId: '',
  };
  private _isMoreDataAvaiable: boolean = true;
  private readonly _untilDestroyed = takeUntilDestroyed(this._destroyRef);

  get getIsLoading() {
    return this._isLoading.asReadonly();
  }

  //methods

  public lazyGetMessages = () => {
    if (this._isLoading() || !this._isMoreDataAvaiable) return;
    this._isLoading.set(true);

    this._messageService
      .lazyGetMessages(this._requestMessagesData)
      .pipe(this._untilDestroyed)
      .subscribe({
        next: (res) => {
          const data = res as IMessageDTO[];

          this.newMesssages$.next(data);

          this._requestMessagesData.taken += data.length;
          this._isMoreDataAvaiable = data.length > 0;
          this._isLoading.set(false);
        },
        error: (err) => {
          // toDo: show error message
          if (err.status == 404) {
            this._isMoreDataAvaiable = false;
          }

          this._isLoading.set(false);
        },
      });
  };

  public getInitialData = async (): Promise<IInitChatGroupDataDTO | null> => {
    try {
      const res = await firstValueFrom(this._chatGroupService.GetInitChatGroupData());

      this._requestMessagesData = {
        ...this._requestMessagesData,
        chatGroupId: res.chatGroupId,
      };

      return res;
    } catch (error) {
      // toDo: show error message
      return null;
    }
  };
}
