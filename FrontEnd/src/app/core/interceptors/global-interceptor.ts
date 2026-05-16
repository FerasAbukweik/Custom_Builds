import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const globalInterceptor: HttpInterceptorFn = (req, next) => {
  const clonedRequest = req.clone({
    withCredentials: true,
  });

  const router = inject(Router);

  return next(clonedRequest).pipe(
    catchError((err) => {
      if (err.status === 401 && !router.url.includes('login-signup')) {
        router.navigate(['/login-signup']);
      }

      return throwError(() => err);
    })
  );
};