import { inject } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IOrderDto } from '../../DTO/orders-dto';
import { IOrderHistoryStatusDTO } from '../../DTO/order-history-status-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { OrderDetailsDto } from '../../DTO/order-details-dto';

@Injectable({
  providedIn: 'root',
})
export class OrderApiService {
  // DI
  private readonly http = inject(HttpClient);

  // private
  private readonly url = Urls.apiUrl + '/Order';

  // api calls

  add() {
    return this.http.post(this.url + 'Add', {});
  }

  // get all orders
  public LazyGetProcessingOrders(data: ILazyDTO) {
    let params = new HttpParams();
    Object.entries(data).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this.http.get<IOrderDto[]>(`${this.url}/GetPendingOrders`, { params });
  }

  // get processing orders count
  getProcessingOrders() {
    return this.http.get<number>(this.url + '/GetPendingOrdersCount');
  }

  // get all completed orders
  public lazyGetOrders(data: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(data).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this.http.get<IOrderDto[]>(`${this.url}`, { params });
  }

  // get completed orders count
  public getHistorySummary() {
    return this.http.get<IOrderHistoryStatusDTO>(`${this.url}/GetHistorySummary`);
  }

  // get order details
  getOrderDetails(orderId: string) {
    let params = new HttpParams();

    params = params.append('orderId', orderId);

    return this.http.get<OrderDetailsDto>(this.url + '/GetOrderDetails', { params });
  }
}
