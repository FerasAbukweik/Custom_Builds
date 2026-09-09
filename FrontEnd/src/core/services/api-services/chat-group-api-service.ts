import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Urls } from 'src/core/constants/urls';

@Injectable({ providedIn: 'root' })
export class ChatGroupApiService {
  // DI
  private readonly http = inject(HttpClient);

  // private
  private readonly url = Urls.apiUrl + '/ChatGroup';

  // api calls

  getChatGroupssById(chatGroupId: string) {
    return this.http.get(`${this.url}/${chatGroupId}`);
  }
}
