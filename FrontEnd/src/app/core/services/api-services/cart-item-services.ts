import { inject, Injectable } from '@angular/core';
import { ApiConstrants } from '../../constants/api-constants';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IAddCustomBuildDTO } from '../../DTO/add-custom-build-dto';
import { ILazyGetCartItemsDTO } from '../../DTO/lazy-get-cart-items-dto';
import { IMiniCartItemDTO } from '../../DTO/mini-cart-item-dto';
import { INewQuantities } from '../../../features/cart.component/cart.model';
import { ICartSummaryInfo } from '../../DTO/cart-summary-info-dto';

@Injectable({ providedIn: 'root' })
export class CartItemServices {
  private url: string = ApiConstrants.url + '/CartItem';
  private httpClient = inject(HttpClient);

  // add customBuild
  public addCustomBuild = (cartItemDTO: IAddCustomBuildDTO) => {
    return this.httpClient.post(`${this.url}/AddCustomBuild`, cartItemDTO);
  };

  // lazy get cart items
  public GetCartItems = (requestData: ILazyGetCartItemsDTO) => {
    let params = new HttpParams();

    Object.entries(requestData).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this.httpClient.get<IMiniCartItemDTO[]>(`${this.url}/GetCartItems`, { params });
  };

  // update quantity
  public updateQuantity = (data: INewQuantities) => {
    const body = {
      newQiantities: data,
    };

    return this.httpClient.put(`${this.url}/updateQuantity`, body);
  };

  // remove item
  public remove = (id: string) => {
    return this.httpClient.delete(`${this.url}/Remove/${id}`);
  };

  // get summary data
  public GetSummaryInfo = () => {
    return this.httpClient.get<ICartSummaryInfo>(`${this.url}/GetSummaryInfo`);
  };
}
