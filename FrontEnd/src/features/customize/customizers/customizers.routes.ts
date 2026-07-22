import { Routes } from '@angular/router';
import { CustomBuildTypeEnum } from '../../../core/enums/custom-build-type-enum';

export const routes: Routes = [
  {
    path: 'controller',
    loadComponent: () =>
      import('./components/controller-customizer.component/controller-customizer.component').then(
        (x) => x.ControllerCustomizerComponent,
      ),
    data: {
      currPage: CustomBuildTypeEnum.Controller,
    },
  },
  {
    path: '**',
    redirectTo: '',
  },
];
