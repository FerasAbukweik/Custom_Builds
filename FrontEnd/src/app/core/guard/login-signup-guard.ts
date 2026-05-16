import { inject } from "@angular/core";
import { CanMatchFn, Router } from "@angular/router";
import { AccountServices } from "../services/account-services";
import { map } from "rxjs";

export const loginSignupGuard : CanMatchFn = () => {
    const accountServices = inject(AccountServices);
    const router = inject(Router);
    
    accountServices.checkToken().pipe(
      map(() => {
        router.navigate(['/']);
      }))
      .subscribe();

    return true;
}