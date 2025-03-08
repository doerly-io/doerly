import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

const ACCESS_TOKEN = 'access_token';

@Injectable({ providedIn: 'root' })
export class JwtTokenHelper {
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.isLoggedIn());

  isLoggedIn$ = this.isLoggedInSubject.asObservable();

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
      this.removeToken();
    }
  }

  getToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN) || sessionStorage.getItem(ACCESS_TOKEN);
  }

  setToken(token: string, isLocalStorage: boolean = true): void {
    if (isLocalStorage) {
      localStorage.setItem(ACCESS_TOKEN, token);
    } else {
      sessionStorage.setItem(ACCESS_TOKEN, token);
    }
    this.isLoggedInSubject.next(this.isLoggedIn());
  }

  removeToken(): void {
    localStorage.removeItem(ACCESS_TOKEN);
    sessionStorage.removeItem(ACCESS_TOKEN);
    this.isLoggedInSubject.next(this.isLoggedIn());
  }
}
