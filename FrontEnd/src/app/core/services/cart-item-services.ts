import { inject, Injectable } from '@angular/core';
import { ApiConstrants } from '../constants/api-constants';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IAddCustomBuildDTO } from '../DTO/add-custom-build-dto';
import { ILazyGetCartItemsDTO } from '../DTO/lazy-get-cart-items-dto';
import { ICartItemDTO } from '../DTO/cart-item-dto';
import { INewQuantities } from '../../features/cart.component/cart.model';

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

    return this.httpClient.get<ICartItemDTO[]>(`${this.url}/GetCartItems`, {params});
  };


  // update quantity
  public updateQuantity = (data : INewQuantities) =>{
    const body  = {
      newQiantities: data
    }

    return this.httpClient.put(`${this.url}/updateQuantity`, body);
  }

  
}
