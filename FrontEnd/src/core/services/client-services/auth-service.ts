import { inject, Injectable, signal } from '@angular/core';
import { AuthApiService } from '../api-services/auth-api-service';
import { firstValueFrom, single, tap } from 'rxjs';
import { IUserData } from '../../DTO/userDataDTO';
import { ILoginDTO } from '../../DTO/login-dto';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  // DI
  private readonly _router = inject(Router);
  private readonly _authApiService = inject(AuthApiService);

  // signals
  private _userData = signal<IUserData | null>(null);
  private _loginServerError = signal<string>('');
  private _isAdmin = signal<boolean | null>(null);

  // getters
  get userData() {
    return this._userData.asReadonly();
  }

  get isAdmin() {
    return this._isAdmin.asReadonly();
  }

  get loginServerError() {
    return this._loginServerError.asReadonly();
  }

  // setters

  setUserData(data: IUserData) {
    this._userData.set(data);
  }

  constructor() {
    const localUser = localStorage.getItem('CB_UserData');

    if (!localUser) return;

    this._userData.set(JSON.parse(localUser));
  }

  // methods

  login(loginData: ILoginDTO) {
    this._authApiService.login(loginData).subscribe({
      next: (data) => {
        this._userData.set(data);
        localStorage.setItem('CB_UserData', JSON.stringify(data));
        this._router.navigateByUrl('/');
      },
      error: (err) => {
        this._loginServerError.set(err.error.ErrorMessage || err.error || 'unexpected error');
      },
    });
  }

  async isAuthenticatedAsync() {
    try {
      await firstValueFrom(this._authApiService.isAuthenticated());
      return true;
    } catch {
      return false;
    }
  }

  async isAdminAsync() {
    try {
      if (this._isAdmin() !== null) return this._isAdmin();
      await firstValueFrom(this._authApiService.isAdmin());

      this._isAdmin.set(true);
      return true;
    } catch {
      this._isAdmin.set(false);
      return false;
    }
  }

  resetServerError() {
    this._loginServerError.set('');
  }

  logout() {
    this._authApiService.logout().subscribe({
      next: () => {
        window.location.reload();
        localStorage.removeItem('CB_UserData');
      },
    });
  }
}
