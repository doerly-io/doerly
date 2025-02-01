import {Injectable} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import translationsEn from '../../../../public/i18n/en.json';
import translationsUk from '../../../../public/i18n/uk.json';

const LANGUAGE = 'language';

@Injectable({providedIn: 'root'})
export class I18nHelperService {

  constructor(private translate: TranslateService) {
    this.setLanguage(this.getLanguage());
  }

  setDefaults(): void {
    this.translate.setTranslation('en', translationsEn);
    this.translate.setTranslation('uk', translationsUk);
    this.translate.setDefaultLang('en');
    this.translate.use('en');

    if (!localStorage.getItem(LANGUAGE)) {
      localStorage.setItem(LANGUAGE, 'en');
    }

    this.translate.use(this.getLanguage());
  }

  getLanguage(): string {
    return localStorage.getItem(LANGUAGE) || 'en';
  }

  setLanguage(lang: string): void {
    if (lang === 'uk' || lang === 'en') {
      this.translate.use(lang);
      localStorage.setItem(LANGUAGE, lang);
      return;
    }

    this.translate.use('en');
    localStorage.setItem(LANGUAGE, 'en');
  }
}
