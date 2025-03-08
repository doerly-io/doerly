import {HttpInterceptorFn} from '@angular/common/http';
import {I18nHelperService} from '../helpers/i18n.helper.service';
import {inject} from '@angular/core';

export const i18nInterceptor: HttpInterceptorFn = (req, next) => {

  const i18nHelperService = inject(I18nHelperService);
  const lang = i18nHelperService.getAcceptedLanguageHeader();
  req = req.clone({
    setHeaders: {
      'Accept-Language': lang,
    },
  });

  return next(req);
};
