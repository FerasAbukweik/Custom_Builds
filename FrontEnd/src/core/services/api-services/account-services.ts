import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IRegisterDTO } from '../../DTO/register-dto';
import { ApiConstrants } from '../../constants/api-constants';
import { ILoginDTO } from '../../DTO/login-dto';

@Injectable({ providedIn: 'root' })
export class AccountServices {
  private _api: string = ApiConstrants.apiUrl + '/Account';
  private readonly _httpClient = inject(HttpClient);

  public register = (registerDTO: IRegisterDTO) => {
    return this._httpClient.post(`${this._api}/Register`, registerDTO);
  };

  public login = (loginDTO: ILoginDTO) => {
    return this._httpClient.post(`${this._api}/Login`, loginDTO);
  };

  public checkToken = () => {
    return this._httpClient.get(`${this._api}/CheckToken`);
  };

  // get curr user id
  public getCurrUserId = () => {
    return this._httpClient.get<string>(this._api + '/GetCurrUserId');
  };
}
