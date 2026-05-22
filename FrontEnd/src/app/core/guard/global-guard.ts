import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { AccountServices } from '../services/api-services/account-services';
import { catchError, map, of } from 'rxjs';

export const globalGuard: CanMatchFn = () => {
  const accountServices = inject(AccountServices);
  const router = inject(Router);

  return accountServices.checkToken()
    .pipe(
        map(() => {
            return true;
        }),
        catchError((err) => {
            router.navigate(["login-signup"])
            return of(false);
        })
    )
};
