import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Urls } from '../../constants/urls';
import { HttpParams } from '@angular/common/http';
import { IOrderItemDto } from '../../DTO/order-item-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';

@Injectable({ providedIn: 'root' })
export class OrderItemApiService {
  // DI
  private readonly _http = inject(HttpClient);

  // private
  private readonly _url = Urls.apiUrl + '/OrderItems';

  // api calls

  getOrderItems(orderId: string, lazyData: ILazyDTO) {
    let params = new HttpParams();

    params = params.append('orderId', orderId);
    params = params.append('taken', lazyData.taken);
    params = params.append('sectionSize', lazyData.sectionSize);

    return this._http.get<IOrderItemDto[]>(this._url, { params });
  }
}
