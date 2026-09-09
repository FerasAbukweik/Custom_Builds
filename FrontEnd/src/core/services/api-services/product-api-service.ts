import { inject, Injectable } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient } from '@angular/common/http';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { HttpParams } from '@angular/common/http';
import { IProductDTO } from '../../DTO/product-dto';
import { ProductAddDTO } from 'src/core/DTO/product-add-dto';
import { ProductEditDTO } from 'src/core/DTO/product-edit-dto';

@Injectable({ providedIn: 'root' })
export class ProductApiService {
  // DI
  private readonly httpClient = inject(HttpClient);

  // private
  private readonly url = Urls.apiUrl + '/Product';

  public getAll(reqData: ILazyDTO) {
    let params = new HttpParams();
    Object.entries(reqData).forEach(([key, val]) => {
      params = params.append(key, val);
    });

    return this.httpClient.get<IProductDTO[]>(this.url + '/GetAll', { params });
  }

  remove(productId: string) {
    return this.httpClient.delete(`${this.url}/Remove/${productId}`);
  }

  update() {}

  add(productData: ProductAddDTO) {
    const formData = new FormData();

    Object.entries(productData).forEach(([key, value]) => {
      formData.append(key, value);
    });

    // remove existing images from formData and append them the correct way
    formData.delete('images');

    productData.images.forEach((image) => {
      formData.append('images', image, image.name);
    });

    return this.httpClient.post<string>(`${this.url}/Add`, formData);
  }

  edit(editData: ProductEditDTO){
    return this.httpClient.put<string>(`${this.url}/Edit`, editData);
  }
}
