import { inject } from "@angular/core";
import { CanMatchFn, Router } from "@angular/router";
import { AccountServices } from "../services/account-services";
import { catchError, map, of } from "rxjs";

export const loginSignupGuard : CanMatchFn = () => {
    const accountServices = inject(AccountServices);
    const router = inject(Router);
    
    accountServices.checkToken().pipe(
        map(() => {
            router.navigate(['/']);
            return false;
        })
    ).subscribe();

    return true;
}