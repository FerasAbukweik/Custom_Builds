import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { AuthService } from '../services/client-services/auth-service';

export const globalGuard: CanMatchFn = async () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!(await authService.isAuthenticatedAsync())) {
    router.navigateByUrl('/login-signup');
    // return false;
  }

  return true;
};
