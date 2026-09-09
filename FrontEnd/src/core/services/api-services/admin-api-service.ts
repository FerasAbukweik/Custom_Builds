import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Urls } from '../../constants/urls';
import { DashboardDto } from '../../DTO/dashboard-dto';
import { OrderManagementStatusDto } from '../../DTO/orders-management-status-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { IOrderDto } from '../../DTO/orders-dto';
import { ChatGroupDTO } from 'src/core/DTO/ChatGroupDTO';
import { IMessageDTO } from 'src/core/DTO/message-dto';

@Injectable({ providedIn: 'root' })
export class AdminApiService {
  // DI
  private readonly _http = inject(HttpClient);

  // private
  private url = Urls.apiUrl + '/Admin';

  // api calls

  getDashboardData() {
    return this._http.get<DashboardDto>(this.url + '/GetDashboardData');
  }

  getOrderManagementStatus() {
    return this._http.get<OrderManagementStatusDto>(this.url + '/GetOrderManagementStatus');
  }

  public lazyGetOrders(data: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(data).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this._http.get<IOrderDto[]>(`${this.url}/GetOrders`, { params });
  }

  public lazyGetChatGroups(lazyData: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(lazyData).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this._http.get<ChatGroupDTO[]>(`${this.url}/GetChatGroups`, { params });
  }

  public lazyGetMessages(groupId: string, lazyData: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(lazyData).forEach(([key, val]) => {
      params = params.append(key, val);
    });
    params = params.append('groupId', groupId);

    return this._http.get<IMessageDTO[]>(this.url + '/GetGroupMessages', { params });
  }
}
