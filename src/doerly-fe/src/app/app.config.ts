import {ApplicationConfig, provideZoneChangeDetection} from '@angular/core';
import {provideRouter} from '@angular/router';

import {routes} from './app.routes';
import {providePrimeNG} from 'primeng/config';
import Aura from '@primeng/themes/aura';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {provideTranslateService} from '@ngx-translate/core';
import {provideHttpClient, withFetch, withInterceptors} from '@angular/common/http';
import {bearerTokenInterceptor} from './@core/interceptors/bearer.token.interceptor';
import {i18nInterceptor} from './@core/interceptors/i18n.interceptor';
import {MessageService} from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(
      withFetch(),
      withInterceptors([i18nInterceptor, bearerTokenInterceptor])
    ),
    provideAnimationsAsync(),
    provideZoneChangeDetection({eventCoalescing: true}),
    provideRouter(routes),
    provideTranslateService({
      defaultLanguage: 'en',
    }),
    providePrimeNG({
      ripple: true,
      theme: {
        preset: Aura,
        options: {
          darkModeSelector: '.my-app-dark',
          cssLayer: false
        }
      }
    }),
    MessageService
  ]
};
