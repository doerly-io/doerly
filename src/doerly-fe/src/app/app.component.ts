import {Component} from '@angular/core';
import {MessageService, SharedModule} from 'primeng/api';
import {Button} from 'primeng/button';
import {MessageModule} from 'primeng/message';
import {RouterOutlet} from '@angular/router';
import {Divider} from 'primeng/divider';
import {I18nHelperService} from './@core/helpers/i18n.helper.service';
import {StyleClass} from 'primeng/styleclass';

const THEME = 'theme';

@Component({
  selector: 'app-root',
  imports: [Button, RouterOutlet, SharedModule, Divider, StyleClass],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers: [MessageService, MessageModule,],
})
export class AppComponent {

  lang!: string;
  theme!: string;

  constructor(private i18nHelperService: I18nHelperService) {
    this.setDefaults();
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

}
