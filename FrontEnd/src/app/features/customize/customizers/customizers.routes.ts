import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'controller',
    loadComponent: () =>
      import('./components/controller-customizer.component/controller-customizer.component').then(
        (x) => x.ControllerCustomizerComponent,
      ),
  },
  {
    path: '**',
    redirectTo: '',
  },
];
