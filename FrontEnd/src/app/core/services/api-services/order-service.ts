import { inject } from '@angular/core';
import { ApiConstrants } from '../../constants/api-constants';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IOrderDTO } from '../../DTO/mini-order-dto'; 
import { ILazyGetAllOrdersDTO } from '../../DTO/lazy-get-all-orders-dto';
import { Injectable } from '@angular/core';
import { IHistoryOrderDTO } from '../../DTO/History-orders-dto';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private readonly api = ApiConstrants.url + '/Order';
  private readonly _httpClient = inject(HttpClient);

  // get all orders
  public getAll = (data: ILazyGetAllOrdersDTO) => {
    let params = new HttpParams();
    Object.entries(data).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this._httpClient.get<IOrderDTO[]>(`${this.api}/GetAll`, { params });
  };

  // get orders count
  public getOrdersCount = (userId: string | null = null) => {
    let params = new HttpParams();

    if(userId) params = params.append('userId', userId);

    return this._httpClient.get<number>(`${this.api}/GetOrdersCount`);
  };

  // get all completed orders
  public getAllCompletedOrders = (data: ILazyGetAllOrdersDTO) => {
    let params = new HttpParams();

    Object.entries(data).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this._httpClient.get<IHistoryOrderDTO[]>(`${this.api}/GetAllCompletedOrders`, { params });
  };

  // get completed orders count
  public getCompletedOrdersCount = (userId: string | null = null) => {
    let params = new HttpParams();

    if(userId) params = params.append("userId" , userId);

    return this._httpClient.get<number>(`${this.api}/GetAllCompletedOrdersCount` , {params});
  }
}
