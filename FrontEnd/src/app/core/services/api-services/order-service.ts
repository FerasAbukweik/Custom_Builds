import { inject } from '@angular/core';
import { ApiConstrants } from '../../constants/api-constants';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IOrderDTO } from '../../../features/user/content/components/my-orders.component/my-orders.model';
import { ILazyGetAllOrdersDTO } from '../../DTO/lazy-get-all-orders-dto';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private readonly api = ApiConstrants.url + '/Order';
  private readonly _httpClient = inject(HttpClient);

  public GetAll(data: ILazyGetAllOrdersDTO) {
    let params = new HttpParams();
    Object.entries(data).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this._httpClient.get<IOrderDTO[]>(`${this.api}/GetAll`, { params });
  }

  public GetOrdersCount(userId : string | null = null){
    let params = new HttpParams();
    params = params.append("userId" , userId ?? "null");

    return this._httpClient.get<number>(`${this.api}/GetOrdersCount`)
  }
}
