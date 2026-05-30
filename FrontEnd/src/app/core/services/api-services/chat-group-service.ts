import { inject, Injectable } from '@angular/core';
import { ApiConstrants } from '../../constants/api-constants';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ChatGroupService {
  private readonly _api = ApiConstrants.apiUrl + '/ChatGroup';
  private readonly _httpClient = inject(HttpClient);


}
