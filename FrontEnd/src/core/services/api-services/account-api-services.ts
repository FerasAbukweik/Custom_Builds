import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IRegisterDTO } from '../../DTO/register-dto';
import { Urls } from '../../constants/urls';
import { IUserData } from '../../DTO/userDataDTO';

@Injectable({ providedIn: 'root' })
export class AccountApiServices {
  // inject
  private readonly httpClient = inject(HttpClient);

  // private
  private url: string = Urls.apiUrl + '/Account';


  // api calls

  public register(registerDTO: IRegisterDTO) {
    return this.httpClient.post<IUserData>(`${this.url}/Register`, registerDTO);
  }
}
