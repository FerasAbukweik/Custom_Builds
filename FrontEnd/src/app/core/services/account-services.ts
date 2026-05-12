import { inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { IRegisterDTO } from "../DTO/register-dto";
import { ApiConstrants } from "../constants/api-constants";
import { ILoginDTO } from "../DTO/login-dto";

@Injectable({providedIn:'root'})
export class AccountServices {
    private url: string = ApiConstrants.url + "/Account";
    private readonly httpClient = inject(HttpClient);

    public register = (registerDTO: IRegisterDTO) => {
        return this.httpClient.post(`${this.url}/Register`, registerDTO);
    }

    public login = (loginDTO: ILoginDTO) => {
        return this.httpClient.post(`${this.url}/Login`, loginDTO);
    }

    public checkToken = () => {
        return this.httpClient.get(`${this.url}/CheckToken`);
    }

}