import { Routes } from '@angular/router';
import { CustomBuildTypeEnum } from '../../../core/enums/custom-build-type-enum';
import { EmptyComponent } from 'src/shared/components/empty-component/empty-component';

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
          import('../../../shared/components/empty-component/empty-component').then(
            (x) => x.EmptyComponent,
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
