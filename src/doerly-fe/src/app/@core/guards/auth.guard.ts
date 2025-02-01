import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {Injectable} from '@angular/core';
import {JwtTokenHelper} from '../helpers/jwtToken.helper';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
  constructor(private jwtTokenHelper: JwtTokenHelper, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.jwtTokenHelper.isLoggedIn()) {
      return true;
    } else {
      this.router.navigate(['/auth/login'], {queryParams: {returnUrl: state.url}});
      return false;
    }
  }
}
