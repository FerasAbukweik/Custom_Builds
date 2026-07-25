import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { contentTypeInterceptor } from '../core/interceptors/content-type0interceptor';
import { authInterceptor } from '../core/interceptors/auth-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(
      routes,
      withComponentInputBinding(),
      // withRouterConfig({paramsInheritanceStrategy: 'always'})
    ),
    provideZonelessChangeDetection(),
    provideHttpClient(withInterceptors([contentTypeInterceptor, authInterceptor])),
  ],
};
