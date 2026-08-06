import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Urls } from '../../constants/urls';
import { DashboardDto } from '../../DTO/dashboard-dto';
import { OrderManagementStatusDto } from '../../DTO/orders-management-status-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { IOrderDto } from '../../DTO/orders-dto';

@Injectable({ providedIn: 'root' })
export class AdminApiService {
  // DI
  private readonly http = inject(HttpClient);

  // private
  private url = Urls.apiUrl + '/Admin';

  // api calls

  getDashboardData() {
    return this.http.get<DashboardDto>(this.url + '/GetDashboardData');
  }

  getOrderManagementStatus() {
    return this.http.get<OrderManagementStatusDto>(this.url + '/GetOrderManagementStatus');
  }

  public lazyGetOrders(data: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(data).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this.http.get<IOrderDto[]>(`${this.url}/GetPendingOrders`, { params });
  }
}
