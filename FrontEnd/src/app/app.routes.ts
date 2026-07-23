import { Routes } from '@angular/router';
import { CustomizerService } from '../features/customize/customizer.service';
import { DashboardService } from '../layouts/dashboard/dashboard.service';
import { loginSignupGuard } from '../core/guard/login-signup-guard';
import { globalGuard } from '../core/guard/global-guard';

export const routes: Routes = [
  {
    path: 'login-signup',
    pathMatch: 'full',
    canMatch: [loginSignupGuard],
    loadComponent: () =>
      import('../features/login-signup.component/login-signup.component').then(
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
          import('../features/home.component/home.component').then((x) => x.HomeComponent),
        pathMatch: 'full',
      },

      {
        path: 'cart',
        loadComponent: () =>
          import('../features/cart.component/cart.component').then((x) => x.CartComponent),
      },

      {
        path: 'customizer',
        loadChildren: () => import('../features/customize/customize.routes').then((x) => x.routes),
        providers: [CustomizerService],
      },

      {
        path: 'user',
        loadComponent: () =>
          import('.././layouts/dashboard/dashboard.layout').then((x) => x.DashboardLayout),
        loadChildren: () => import('../features/user/user.routes').then((x) => x.routes),
        providers: [DashboardService],
        data: {
          pagesData: [
            { icon: 'fa-solid fa-box-open', name: 'My Orders', goTo: 'my-orders' },
            { icon: 'fa-solid fa-clock-rotate-left', name: 'History', goTo: 'history' },
            { icon: 'fa-solid fa-headset', name: 'Support', goTo: 'support' },
          ],
        },
      },

      {
        path: 'admin',
        loadComponent: () =>
          import('.././layouts/dashboard/dashboard.layout').then((x) => x.DashboardLayout),
        loadChildren: () => import('../features/admin/admin.routes').then((x) => x.routes),
        providers: [DashboardService],
        data: {
          pagesData: [
            { icon: 'fa-solid fa-box-open', name: 'Dashboard', goTo: 'dashboard' },
            {
              icon: 'fa-solid fa-clock-rotate-left',
              name: 'Orders Management',
              goTo: 'orders-management',
            },
            {
              icon: 'fa-solid fa-headset',
              name: 'Inventory Management',
              goTo: 'inventory-management',
            },
            { icon: 'fa-solid fa-headset', name: 'Support Chat', goTo: 'support-chat' },
          ],
        },
      },
    ],
  },

  { path: '**', redirectTo: 'not-found' },
];
