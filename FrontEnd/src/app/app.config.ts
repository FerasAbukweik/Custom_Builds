import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter, withComponentInputBinding, withRouterConfig } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { globalInterceptor } from '../core/interceptors/global-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(
      routes,
      withComponentInputBinding(),
      // withRouterConfig({paramsInheritanceStrategy: 'always'})
    ),
    provideZonelessChangeDetection(),
    provideHttpClient(withInterceptors([globalInterceptor])),
  ],
};
