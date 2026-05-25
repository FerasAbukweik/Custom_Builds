import { inject, Injectable } from '@angular/core';
import { ApiConstrants } from '../../constants/api-constants';
import { HttpClient } from '@angular/common/http';
import { IInitChatGroupDataDTO } from '../../DTO/init-chat-group-data-dto';

@Injectable({ providedIn: 'root' })
export class ChatGroupService {
  private readonly _api = ApiConstrants.apiUrl + '/ChatGroup';
  private readonly _httpClient = inject(HttpClient);

  public GetInitChatGroupData() {
    return this._httpClient.get<IInitChatGroupDataDTO>(this._api + '/GetInitChatGroupData');
  }
}
