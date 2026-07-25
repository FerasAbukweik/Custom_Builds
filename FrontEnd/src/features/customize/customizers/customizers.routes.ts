import { Routes } from '@angular/router';
import { CustomBuildTypeEnum } from '../../../core/enums/custom-build-type-enum';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('.././customizers/layout/customizer.layout/customizer.layout').then(
        (x) => x.CustomizerLayout,
      ),
    children: [
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
    ],
  },
];
