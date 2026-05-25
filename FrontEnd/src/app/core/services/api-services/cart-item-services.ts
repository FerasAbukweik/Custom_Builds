import { inject, Injectable } from '@angular/core';
import { ApiConstrants } from '../../constants/api-constants';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IAddCustomBuildDTO } from '../../DTO/add-custom-build-dto';
import { ILazyGetUserDataDTO } from '../../DTO/lazy-get-user-data-dto';
import { IMiniCartItemDTO } from '../../DTO/mini-cart-item-dto';
import { INewQuantities } from '../../../features/cart.component/cart.model';
import { ICartSummaryInfo } from '../../DTO/cart-summary-info-dto';

@Injectable({ providedIn: 'root' })
export class CartItemServices {
  private _api: string = ApiConstrants.apiUrl + '/CartItem';
  private _httpClient = inject(HttpClient);

  // add customBuild
  public addCustomBuild = (cartItemDTO: IAddCustomBuildDTO) => {
    return this._httpClient.post(`${this._api}/AddCustomBuild`, cartItemDTO);
  };

  // lazy get cart items
  public GetCartItems = (requestData: ILazyGetUserDataDTO) => {
    let params = new HttpParams();

    Object.entries(requestData).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this._httpClient.get<IMiniCartItemDTO[]>(`${this._api}/GetCartItems`, { params });
  };

  // update quantity
  public updateQuantity = (data: INewQuantities) => {
    const body = {
      newQiantities: data,
    };

    return this._httpClient.put(`${this._api}/updateQuantity`, body);
  };

  // remove item
  public remove = (id: string) => {
    return this._httpClient.delete(`${this._api}/Remove/${id}`);
  };

  // get summary data
  public GetSummaryInfo = () => {
    return this._httpClient.get<ICartSummaryInfo>(`${this._api}/GetSummaryInfo`);
  };
}
