import { inject, Injectable } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient } from '@angular/common/http';
import { ILoginDTO } from '../../DTO/login-dto';
import { IUserData } from '../../DTO/userDataDTO';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  // inject
  private readonly http = inject(HttpClient);

  // private
  private readonly url = Urls.apiUrl + '/Auth';

  // api calls

  isAuthenticated() {
    return this.http.post(this.url + '/IsAuthenticated', {});
  }

  login(loginData: ILoginDTO) {
    return this.http.post<IUserData>(this.url + '/Login', loginData);
  }

  logout() {
    return this.http.post(this.url + 'Logout', {});
  }

  updateTokens() {
    return this.http.post(this.url + '/UpdateTokens', {});
  }
}
