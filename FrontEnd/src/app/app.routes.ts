import { Routes } from '@angular/router';
import { CustomizerService } from './features/customize/customizer.service'; 
import { UserPageService } from './features/user/user-page.service';
import { loginSignupGuard } from './core/guard/login-signup-guard';
import { globalGuard } from './core/guard/global-guard';

export const routes: Routes = [
  {
    path: 'login-signup',
    pathMatch: 'full',
    canMatch: [loginSignupGuard],
    loadComponent: () =>
      import('./features/login-signup.component/login-signup.component').then(
        (x) => x.LoginSignupComponent,
      ),
  },

  {
    path: '',
    canMatch: [globalGuard],
    children: [
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
        path: 'user',
        loadComponent: () =>
          import('./features/user/layout/user.layout/user.layout').then((x) => x.UserLayout),
        loadChildren: () => import('./features/user/user.routes').then((x) => x.routes),
        providers: [UserPageService],
      },  
    ]
  },

  { path: '**', redirectTo: 'not-found' },
];
