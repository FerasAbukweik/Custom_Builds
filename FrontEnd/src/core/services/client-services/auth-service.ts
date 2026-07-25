import { inject, Injectable, signal } from '@angular/core';
import { AuthApiService } from '../api-services/auth-api-service';
import { firstValueFrom, tap } from 'rxjs';
import { IUserData } from '../../DTO/userDataDTO';
import { ILoginDTO } from '../../DTO/login-dto';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  // DI
  private readonly router = inject(Router);
  private readonly authApiService = inject(AuthApiService);

  // signals
  userData = signal<IUserData | null>(null);
  loginServerError = signal<string>('');

  // methods

  login(loginData: ILoginDTO) {
    this.authApiService.login(loginData).subscribe({
      next: (data) => {
        this.userData.set(data);
        this.router.navigateByUrl('/');
      },
      error: (err) => {
        this.loginServerError.set(err.error.ErrorMessage || err.error || 'unexpected error');
      },
    });
  }

  async isAuthenticatedAsync() {
    try {
      await firstValueFrom(this.authApiService.isAuthenticated());
      return true;
    } catch {
      return false;
    }
  }
}
