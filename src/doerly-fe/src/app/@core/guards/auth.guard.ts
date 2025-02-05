import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {JwtTokenHelper} from '../helpers/jwtToken.helper';
import {AuthService} from '../../modules/authorization/domain/auth.service';

export const authGuard: CanActivateFn = (route, state) => {

  const jwtTokenHelper = inject(JwtTokenHelper);
  const router = inject(Router);
  if (jwtTokenHelper.isLoggedIn()) {
    return true;
  } else {
    const authService = inject(AuthService);
    authService.refreshToken().subscribe({
      next: (result) => {
        jwtTokenHelper.setToken(result.value!.accessToken);
        router.navigate([state.url]);
        return true;
      },
      error: (error) => {
        router.navigate(['/auth/login'], {queryParams: {returnUrl: state.url}});
        return false;
      }
    });

    return false;
  }
}
