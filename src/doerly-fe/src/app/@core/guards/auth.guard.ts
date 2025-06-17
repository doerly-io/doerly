import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {JwtTokenHelper} from '../helpers/jwtToken.helper';
import {AuthService} from 'app/modules/authorization/domain/auth.service';

export const authGuard: CanActivateFn = async (route, state) => {

  const jwtTokenHelper = inject(JwtTokenHelper);
  const router = inject(Router);
  if (jwtTokenHelper.isLoggedIn()) {
    return true;
  } else {

    const authService = inject(AuthService);
    try {
      const refreshTokenResult = await authService.refreshToken().toPromise();
      if (refreshTokenResult != undefined && refreshTokenResult.isSuccess) {
        jwtTokenHelper.removeToken();
        jwtTokenHelper.setToken(refreshTokenResult.value!.accessToken);
        return true;
      }

      await router.navigate(['/auth/login'], {queryParams: {returnUrl: state.url}});
      return false;
    } catch (error) {
      console.error(error);
      jwtTokenHelper.removeToken();
      await router.navigate(['/auth/login'], {queryParams: {returnUrl: state.url}});
      return false;
    }
  }
}
