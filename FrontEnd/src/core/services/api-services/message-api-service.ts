import { inject, Injectable } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IMessageDTO } from '../../DTO/message-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';

@Injectable({ providedIn: 'root' })
export class MessagesApiService {
  private _api = Urls.apiUrl + '/Message';
  private readonly _httpCLient = inject(HttpClient);

  public lazyGetMessages(reqData: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(reqData).forEach(([key, val]) => {
      params = params.append(key, val.toString());
    });

    return this._httpCLient.get<IMessageDTO[]>(this._api + '/GetMessages', { params });
  }
}
