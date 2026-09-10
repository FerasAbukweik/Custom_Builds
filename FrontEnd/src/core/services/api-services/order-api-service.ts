import { inject } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IOrderDto } from '../../DTO/orders-dto';
import { IOrderHistoryStatusDTO } from '../../DTO/order-history-status-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { OrderDetailsDto } from '../../DTO/order-details-dto';
import { OrderStateEnum } from 'src/core/enums/order-status-enum';

@Injectable({
  providedIn: 'root',
})
export class OrderApiService {
  // DI
  private readonly _http = inject(HttpClient);

  // private
  private readonly _url = Urls.apiUrl + '/Order';

  // api calls

  add() {
    return this._http.post(this._url + '/Add', {});
  }

  // get all orders
  public LazyGetProcessingOrders(data: ILazyDTO) {
    let params = new HttpParams();
    Object.entries(data).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this._http.get<IOrderDto[]>(`${this._url}/GetPendingOrders`, { params });
  }

  // get processing orders count
  getProcessingOrders() {
    return this._http.get<number>(this._url + '/GetPendingOrdersCount');
  }

  // get all completed orders
  public lazyGetOrders(data: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(data).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this._http.get<IOrderDto[]>(`${this._url}`, { params });
  }

  // get completed orders count
  public getHistorySummary() {
    return this._http.get<IOrderHistoryStatusDTO>(`${this._url}/GetHistorySummary`);
  }

  // get order details
  getOrderDetails(orderId: string) {
    let params = new HttpParams();

    params = params.append('orderId', orderId);

    return this._http.get<OrderDetailsDto>(this._url + '/GetOrderDetails', { params });
  }

  updateStatus(orderId: string, newStatus: OrderStateEnum) {
    return this._http.put(this._url + `/UpdateStatus/${orderId}`, newStatus);
  }
}
