import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { AuthApiService } from '../services/api-services/auth-api-service';

export const loginSignupGuard: CanMatchFn = () => {
  const authService = inject(AuthApiService);
  const router = inject(Router);

  authService.isAuthenticated().subscribe({
    next: () => {
      router.navigateByUrl('/');
    },
  });

  return true;
};
