import { Routes } from '@angular/router';
import { HistoryService } from './content/components/history.component/history.service';
import { MyOrdersService } from './content/components/my-orders.component/my-orders.service';
import { SupportService } from './content/components/support.component/support.service';

export const routes: Routes = [
  {
    path: 'my-orders',
    loadComponent: () =>
      import('./content/components/my-orders.component/my-orders.component').then(
        (x) => x.MyOrdersComponent,
      ),
      providers: [MyOrdersService]
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
      import('./content/components/history.component/history.component').then(
        (x) => x.HistoryComponent,
      ),
      providers: [HistoryService]
  },

  {
    path: 'support',
    loadComponent: () =>
      import('./content/components/support.component/support.component').then(
        (x) => x.SupportComponent,
      ),
      providers: [SupportService]
  },
  {
    path: '**',
    redirectTo: 'my-orders',
  },
];
