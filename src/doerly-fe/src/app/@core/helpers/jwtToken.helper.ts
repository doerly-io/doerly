import {Injectable} from '@angular/core';

const ACCESS_TOKEN = 'access_token';

@Injectable({providedIn: 'root'})
export class JwtTokenHelper {

  isLoggedIn(): boolean {
    return !this.isTokenExpired();
  }

  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) {
      return true;
    }
    const payload = this.getPayload(token);
    return payload.exp < Date.now() / 1000;
  }

  getPayload(token: string): any {
    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch (e) {
      localStorage.removeItem(ACCESS_TOKEN);
    }
  }

  getToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN);
  }

  setToken(token: string): void {
    localStorage.setItem(ACCESS_TOKEN, token);
  }

  removeToken(): void {
    localStorage.removeItem(ACCESS_TOKEN);
  }

}
