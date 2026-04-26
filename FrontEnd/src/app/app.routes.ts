import { Routes } from '@angular/router';
import { CustomizerService } from './features/customize/customizers/services/customizer.service';
import { UserPageService } from './features/user/user-page.service';
import { LoginSignupService } from './features/login-signup.component/login-signup.service';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/home.component/home.component').then((x) => x.HomeComponent),
    pathMatch: 'full',
  },
  {
    path: 'cart',
    loadComponent: () =>
      import('./features/cart.component/cart.component').then((x) => x.CartComponent),
  },
  {
    path: 'customizer',
    loadChildren: () => import('./features/customize/customize.routes').then((x) => x.routes),
    providers: [CustomizerService],
  },
  {
    path: 'login-signup',
    loadComponent: () =>
      import('./features/login-signup.component/login-signup.component').then(
        (x) => x.LoginSignupComponent,
      ),
    providers: [LoginSignupService],
  },
  {
    path: 'user',
    loadComponent: () =>
      import('./features/user/layout/user.layout/user.layout').then((x) => x.UserLayout),
    loadChildren: () => import('./features/user/user.routes').then((x) => x.routes),
    providers: [UserPageService],
  },
  { path: '**', redirectTo: '' },
];
