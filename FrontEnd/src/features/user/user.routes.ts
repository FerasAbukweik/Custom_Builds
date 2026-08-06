import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'my-orders',
    loadComponent: () =>
      import('./pages/components/my-orders/my-orders.component').then(
        (x) => x.MyOrdersComponent,
      ),
  },
  // {
  //   path: 'saved-builds',
  //   loadComponent: () =>
  //     import('./content/components/saved-builds.component/saved-builds.component').then(
  //       (x) => x.SavedBuildsComponent,
  //     ),
  // },
  {
    path: 'history',
    loadComponent: () =>
      import('./pages/components/history/history.component').then(
        (x) => x.HistoryComponent,
      ),
  },

  {
    path: 'support',
    loadComponent: () =>
      import('./pages/components/support/support.component').then(
        (x) => x.SupportComponent,
      ),
  },
  {
    path: '**',
    redirectTo: 'my-orders',
  },
];
