import {Component, OnInit} from '@angular/core';
import {MessageService, SharedModule} from 'primeng/api';
import {Button} from 'primeng/button';
import {MessageModule} from 'primeng/message';
import {RouterLink, RouterOutlet} from '@angular/router';
import {Divider} from 'primeng/divider';
import {I18nHelperService} from './@core/helpers/i18n.helper.service';
import {StyleClass} from 'primeng/styleclass';
import {TranslatePipe} from '@ngx-translate/core';
import {JwtTokenHelper} from './@core/helpers/jwtToken.helper';
import {NgIf} from '@angular/common';
import {Popover} from 'primeng/popover';
import {AuthService} from './modules/authorization/domain/auth.service';

const THEME = 'theme';

@Component({
  selector: 'app-root',
  imports: [Button, RouterOutlet, SharedModule, Divider, StyleClass, RouterLink, TranslatePipe, NgIf, Popover],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers: [MessageService, MessageModule,],
})
export class AppComponent implements OnInit {

  lang!: string;
  theme!: string;
  isLoggedIn: boolean = false;

  constructor(private i18nHelperService: I18nHelperService,
              private jwtTokenHelper: JwtTokenHelper,
              private authService: AuthService,
  ) {
    this.setDefaults();
  }

  ngOnInit(): void {
    this.jwtTokenHelper.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
    });
  }

  setDefaults() {
    this.i18nHelperService.setDefaults();
    this.loadTheme();
    this.lang = this.i18nHelperService.getLanguage();
  }

  setLanguage(lang: string) {
    this.i18nHelperService.setLanguage(lang);
    this.lang = this.i18nHelperService.getLanguage();
  }

  loadTheme() {
    this.theme = localStorage.getItem(THEME) || 'light';
    if (this.theme === 'dark') {
      document.querySelector('html')!.classList.add('my-app-dark');
    } else {
      document.querySelector('html')!.classList.remove('my-app-dark');
    }
  }

  toggleDarkMode() {
    const html = document.querySelector('html')!;
    const theme = html.classList.contains('my-app-dark') ? 'light' : 'dark';
    localStorage.setItem(THEME, theme);
    this.loadTheme();
  }

  logout() {
    this.authService.logout();
    this.isLoggedIn = false;
    this.jwtTokenHelper.removeToken();
    window.location.reload();
  }

}
