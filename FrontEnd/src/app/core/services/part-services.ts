import { inject, Injectable } from "@angular/core";
import { ApiConstrants } from "../constants/api-constants";
import { HttpClient } from "@angular/common/http";
import { IPart } from "../interfaces/customize-data/customize-data.model";

@Injectable({ providedIn: 'root' })
export class PartServices {
    private readonly url = ApiConstrants.url + "/Part";
    private readonly http = inject(HttpClient);

    public getAllParts(){
        return this.http.get<IPart[]>(`${this.url}/GetAllParts`);
    }
}