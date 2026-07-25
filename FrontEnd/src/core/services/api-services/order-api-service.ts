import { inject } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IOrderDTO } from '../../DTO/mini-order-dto';
import { Injectable } from '@angular/core';
import { IHistoryOrderDTO } from '../../DTO/History-orders-dto';
import { IOrderHistorySummaryDTO } from '../../DTO/order-history-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';

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
  public getAllProcessingOrders(data: ILazyDTO) {
    let params = new HttpParams();
    Object.entries(data).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this.http.get<IOrderDTO[]>(`${this.url}/GetAllProcessingOrders`, { params });
  }

  // get all completed orders
  public lazyGetCompletedOrders(data: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(data).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this.http.get<IHistoryOrderDTO[]>(`${this.url}/GetAllCompletedOrders`, {
      params,
    });
  }

  // get orders count
  public getProcessingOrdersCount() {
    return this.http.get<number>(`${this.url}/GetProcessingOrdersCount`);
  }

  // get completed orders count
  public getHistorySummary() {
    return this.http.get<IOrderHistorySummaryDTO>(`${this.url}/GetHistorySummary`);
  }

  // buy order again
  public buyAgain(orderItemId: string) {
    return this.http.post(this.url + '/BuyAgain', JSON.stringify(orderItemId));
  }
}
