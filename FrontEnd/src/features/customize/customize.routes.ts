import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./select-customizer/select-customizer.component').then(
        (x) => x.SelectCustomizerComponent,
      ),
  },
  {
    path: '',
    loadChildren: () => import('./customizers/customizers.routes').then((x) => x.routes),
  },
];
