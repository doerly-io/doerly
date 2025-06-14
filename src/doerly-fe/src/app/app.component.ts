import {Component, OnInit} from '@angular/core';
import {MessageService, SharedModule} from 'primeng/api';
import {Button, ButtonDirective} from 'primeng/button';
import {MessageModule} from 'primeng/message';
import {RouterLink, RouterOutlet, Router, NavigationEnd} from '@angular/router';
import {Divider} from 'primeng/divider';
import {I18nHelperService} from './@core/helpers/i18n.helper.service';
import {StyleClass} from 'primeng/styleclass';
import {TranslatePipe} from '@ngx-translate/core';
import {JwtTokenHelper} from './@core/helpers/jwtToken.helper';
import {NgIf} from '@angular/common';
import {Popover} from 'primeng/popover';
import {AuthService} from './modules/authorization/domain/auth.service';
import {Toast} from 'primeng/toast';
import {CategoryDropdownComponent} from './@components/category-dropdown/category-dropdown.component';
import {NotificationBadgeComponent} from './@components/notification-badge/notification-badge.component';
import {NotificationPanelComponent} from './@components/notification-panel/notification-panel.component';
import {filter} from 'rxjs';
import {SearchService} from './@core/services/search.service';
import {FormsModule} from '@angular/forms';

const THEME = 'theme';

@Component({
  selector: 'app-root',
  imports: [
    Button,
    RouterOutlet,
    SharedModule,
    Divider,
    StyleClass,
    RouterLink,
    TranslatePipe,
    NgIf,
    Popover,
    Toast,
    CategoryDropdownComponent,
    NotificationBadgeComponent,
    NotificationPanelComponent,
    ButtonDirective,
    FormsModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers: [MessageModule],
})
export class AppComponent implements OnInit {

  lang!: string;
  theme!: string;
  isLoggedIn: boolean = false;
  isAuthPage: boolean = false;
  currentYear: number = new Date().getFullYear();

  constructor(private i18nHelperService: I18nHelperService,
              private jwtTokenHelper: JwtTokenHelper,
              private authService: AuthService,
              private router: Router,
              private searchService: SearchService) {
    this.setDefaults();
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      this.isAuthPage = event.url.includes('/auth');
    });
  }

  ngOnInit(): void {
    this.jwtTokenHelper.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
    });
    this.isAuthPage = this.router.url.includes('/auth');
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

  // onSearch() {
  //   if (this.searchText.trim()) {
  //     this.searchService.setSearchValue(this.searchText);
  //     this.router.navigate(['/catalog']);
  //   }
  // }

}
