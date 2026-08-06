import { inject, Injectable } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient } from '@angular/common/http';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { HttpParams } from '@angular/common/http';
import { IProductDTO } from '../../DTO/product-dto';

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

  remove(productId: string){
    return this.httpClient.delete(`${this.url}/Remove/${productId}`);
  }

  update(){
    
  }
}
