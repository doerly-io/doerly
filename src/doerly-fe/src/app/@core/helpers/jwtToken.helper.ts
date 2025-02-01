import {Injectable} from '@angular/core';

const ACCESS_TOKEN = 'access_token';

@Injectable({providedIn: 'root'})
export class JwtTokenHelper {

  isLoggedIn(): boolean {
    return !!localStorage.getItem(ACCESS_TOKEN);
  }

  isTokenExpired(): boolean {
    const token = localStorage.getItem(ACCESS_TOKEN);
    if (!token) {
      return true;
    }
    const payload = this.getPayload(token);
    return payload.exp < Date.now() / 1000;
  }

  getPayload(token: string): any {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload));
  }

  getToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN);
  }

  setToken(token: string): void {
    localStorage.setItem(ACCESS_TOKEN, token);
  }

}
