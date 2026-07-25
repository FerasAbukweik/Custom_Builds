import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthApiService } from '../services/api-services/auth-api-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const cloned = req.clone({
    withCredentials: true,
  });

  const router = inject(Router);
  const authApiService = inject(AuthApiService);

  return next(cloned).pipe(
    catchError((err) => {
      if (err.status === 401 && !router.url.includes('login-signup')) {
        return authApiService.updateTokens().pipe(
          switchMap(() => {
            return next(cloned);
          }),
          catchError((err) => {
            router.navigateByUrl('/login-signup');
            return throwError(() => err);
          }),
        );
      }

      return throwError(() => err);
    }),
  );
};
