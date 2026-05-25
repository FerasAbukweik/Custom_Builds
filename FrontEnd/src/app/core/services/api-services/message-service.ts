import { inject, Injectable } from '@angular/core';
import { ApiConstrants } from '../../constants/api-constants';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IMessageDTO } from '../../DTO/message-dto';
import { ILazyLoadMessagesDTO } from '../../DTO/lazy-load-messages-dto';

@Injectable({ providedIn: 'root' })
export class MessagesService {
  private _api = ApiConstrants.apiUrl + '/Message';
  private readonly _httpCLient = inject(HttpClient);

  public lazyGetMessages(reqData: ILazyLoadMessagesDTO) {
    let params = new HttpParams();

    Object.entries(reqData).forEach(([key, val]) => {
      params = params.append(key, val.toString());
    });

    return this._httpCLient.get<IMessageDTO[]>(this._api + '/GetMessages', { params });
  }
}
