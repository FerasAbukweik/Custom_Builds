import { inject, Injectable, signal } from '@angular/core';
import { ChatGroupDTO } from 'src/core/DTO/ChatGroupDTO';
import { ILazyDTO } from 'src/core/DTO/lazy-dto';
import { AdminApiService } from 'src/core/services/api-services/admin-api-service';

@Injectable({ providedIn: 'root' })
export class ChatGroupService {
  // DI
  private readonly adminApiService = inject(AdminApiService);

  // signals
  private _chatGroups = signal<ChatGroupDTO[]>([]);
  private _isLoading = signal<boolean>(false);

  // priavte
  private _lazyData: ILazyDTO = {
    taken: 0,
    sectionSize: 10,
  };
  private _isMoreDataAvailable: boolean = true;

  // getters
  get chatGroups() {
    return this._chatGroups.asReadonly();
  }

  get isLoading() {
    return this._isLoading.asReadonly();
  }

  // methods

  lazyGetChatGroups() {
    if (this.isLoading() || !this._isMoreDataAvailable) return;
    this._isLoading.set(true);

    this.adminApiService.lazyGetChatGroups(this._lazyData).subscribe({
      next: (data) => {
        this._chatGroups.update((curr) => [...curr, ...data]);
        this._isMoreDataAvailable = data.length > 0;
        this._lazyData.taken += data.length;

        this._isLoading.set(false);
      },
      error: () => {
        this._isLoading.set(false);
      },
    });
  }
}
