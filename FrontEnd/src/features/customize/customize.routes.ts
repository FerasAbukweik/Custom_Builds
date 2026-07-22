import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./select-customizer.component/select-customizer.component').then(
        (x) => x.SelectCustomizerComponent,
      ),
  },
  {
    path: '',
    loadComponent: () =>
      import('./customizers/layout/customizer.layout/customizer.layout').then(
        (x) => x.CustomizerLayout,
      ),
    loadChildren: () => import('./customizers/customizers.routes').then((x) => x.routes),
  },
];
