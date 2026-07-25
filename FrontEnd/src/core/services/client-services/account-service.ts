import { inject, Injectable, signal } from '@angular/core';
import { AccountApiServices } from '../api-services/account-api-services';
import { IRegisterDTO } from '../../DTO/register-dto';
import { AuthService } from './auth-service';
import { tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AccountService {
  // DI
  private readonly accountApiService = inject(AccountApiServices);
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);

  // signals
  registerServerError = signal<string>('');

  // methods

  register(registerData: IRegisterDTO) {
    return this.accountApiService.register(registerData).subscribe({
      next: (data) => {
        this.authService.userData.set(data);
        this.router.navigateByUrl('/');
      },
      error: (err) => {
        this.registerServerError.set(err.error.ErrorMessage || err.error || 'unexpected error');
      },
    });
  }
}
