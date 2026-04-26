import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'my-orders',
    loadComponent: () =>
      import('./content/components/my-orders.component/my-orders.component').then(
        (x) => x.MyOrdersComponent,
      ),
  },
  {
    path: 'saved-builds',
    loadComponent: () =>
      import('./content/components/saved-builds.component/saved-builds.component').then(
        (x) => x.SavedBuildsComponent,
      ),
  },
  {
    path: 'history',
    loadComponent: () =>
      import('./content/components/history.component/history.component').then(
        (x) => x.HistoryComponent,
      ),
  },

  {
    path: 'support',
    loadComponent: () =>
      import('./content/components/support.component/support.component').then(
        (x) => x.SupportComponent,
      ),
  },
  {
    path: '**',
    redirectTo: 'my-orders',
  },
];
