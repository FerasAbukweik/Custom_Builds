import { inject } from '@angular/core';
import { ApiConstrants } from '../../constants/api-constants';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IOrderDTO } from '../../DTO/mini-order-dto';
import { ILazyGetAllOrdersDTO } from '../../DTO/lazy-get-all-orders-dto';
import { Injectable } from '@angular/core';
import { IHistoryOrderDTO } from '../../DTO/History-orders-dto';
import { IOrderHistoryDTO } from '../../DTO/order-history-dto';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private readonly api = ApiConstrants.url + '/Order';
  private readonly _httpClient = inject(HttpClient);

  // get all orders
  public getAllProcessingOrders = (data: ILazyGetAllOrdersDTO) => {
    let params = new HttpParams();
    Object.entries(data).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this._httpClient.get<IOrderDTO[]>(`${this.api}/GetAllProcessingOrders`, { params });
  };

  // get orders count
  public getProcessingOrdersCount = (userId: string | null = null) => {
    let params = new HttpParams();

    if (userId) params = params.append('userId', userId);

    return this._httpClient.get<number>(`${this.api}/GetProcessingOrdersCount`);
  };

  // get all completed orders
  public getAllCompletedOrders = (data: ILazyGetAllOrdersDTO) => {
    let params = new HttpParams();

    Object.entries(data).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this._httpClient.get<IHistoryOrderDTO[]>(`${this.api}/GetAllCompletedOrders`, {
      params,
    });
  };

  // get completed orders count
  public getHistorySummary = (userId: string | null = null) => {
    let params = new HttpParams();

    if (userId) params = params.append('userId', userId);

    return this._httpClient.get<IOrderHistoryDTO>(`${this.api}/GetHistorySummary`, { params });
  };

  // buy order again
  public buyAgain(orderId: string){

    return this._httpClient.post(this.api+"/BuyAgain" , JSON.stringify(orderId));
  }
}
