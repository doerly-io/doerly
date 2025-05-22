import {HttpInterceptorFn} from '@angular/common/http';
import {JwtTokenHelper} from '../helpers/jwtToken.helper';
import {inject} from '@angular/core';

export const bearerTokenInterceptor: HttpInterceptorFn = (req, next) => {

  if (req.url.includes('login') || req.url.includes('register')) {
    return next(req);
  }

  const jwtTokenHelper = inject(JwtTokenHelper);
  const jwtToken = jwtTokenHelper.getToken();
  if (!jwtToken) {
    return next(req);
  }
  req = req.clone({
    setHeaders: {
      Authorization: `Bearer ${jwtToken}`
    }
  });

  return next(req);

};
