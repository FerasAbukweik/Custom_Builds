import { inject, Injectable } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ICustomBuildAddDTO } from '../../DTO/add-custom-build-dto';
import { IMiniCartItemDTO } from '../../DTO/mini-cart-item-dto';
import { INewQuantities } from '../../../features/cart/cart.model';
import { ICartSummaryInfo } from '../../DTO/cart-summary-info-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';

@Injectable({ providedIn: 'root' })
export class CartItemApiServices {
  // DI
  private httpClient = inject(HttpClient);

  // private
  private url: string = Urls.apiUrl + '/CartItem';

  // api calls

  // add product
  addProduct(productId: string) {
    return this.httpClient.post(this.url + '/AddProduct', JSON.stringify(productId));
  }

  // add customBuild
  addCustomBuild(customBuildData: ICustomBuildAddDTO) {
    return this.httpClient.post(`${this.url}/AddCustomBuild`, customBuildData);
  }

  // remove item
  public remove(id: string) {
    return this.httpClient.delete(`${this.url}/Remove/${id}`);
  }

  // lazy get cart items
  public GetCartItems(requestData: ILazyDTO) {
    let params = new HttpParams();

    Object.entries(requestData).forEach(([key, value]) => {
      params = params.append(key, value.toString());
    });

    return this.httpClient.get<IMiniCartItemDTO[]>(`${this.url}/GetCartItems`, { params });
  }

  // update quantity
  public updateQuantity(data: INewQuantities) {
    return this.httpClient.put(`${this.url}/updateQuantity`, data);
  }

  // get summary data
  public GetSummaryInfo() {
    return this.httpClient.get<ICartSummaryInfo>(`${this.url}/GetSummaryInfo`);
  }
}
