import { inject, Injectable } from "@angular/core";
import { ApiConstrants } from "../../constants/api-constants";
import { HttpClient } from "@angular/common/http";
import { ILazyLoadingDTO } from "../../DTO/lazy-loading-dto";
import { HttpParams } from "@angular/common/http";
import { IProductDTO } from "../../DTO/product-dto";

@Injectable({providedIn: 'root'})
export class ProductService{
    private readonly _api = ApiConstrants.apiUrl + "/Product";
    private readonly _httpClient = inject(HttpClient);

    public getAll = (reqData: ILazyLoadingDTO) => {
        let params = new HttpParams();
        Object.entries(reqData).forEach(([key , val]) => {
            params = params.append(key , val);
        })

        return this._httpClient.get<IProductDTO[]>(this._api + "/GetAll" , {params});
    }
}