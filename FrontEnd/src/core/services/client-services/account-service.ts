import { inject, Injectable, signal } from '@angular/core';
import { AccountApiServices } from '../api-services/account-api-services';
import { IRegisterDTO } from '../../DTO/register-dto';
import { AuthService } from './auth-service';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AccountService {
  // DI
  private readonly _accountApiService = inject(AccountApiServices);
  private readonly _router = inject(Router);
  private readonly _authService = inject(AuthService);

  // signals
  registerServerError = signal<string>('');

  // methods

  register(registerData: IRegisterDTO) {
    return this._accountApiService.register(registerData).subscribe({
      next: (data) => {
        this._authService.userData.set(data);
        this._router.navigateByUrl('/');
      },
      error: (err) => {
        this.registerServerError.set(err.error.ErrorMessage || err.error || 'unexpected error');
      },
    });
  }
}
