import { HttpInterceptorFn } from '@angular/common/http';

export const i18nInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req);
};
