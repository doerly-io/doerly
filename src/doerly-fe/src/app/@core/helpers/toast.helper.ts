import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ToastHelper {
  constructor(
    private messageService: MessageService,
    private translateService: TranslateService
  ) {}

  showSuccess(summaryKey: string, detailKey: string, params?: any): void {
    this.showToast('success', summaryKey, detailKey, params);
  }

  showInfo(summaryKey: string, detailKey: string, params?: any): void {
    this.showToast('info', summaryKey, detailKey, params);
  }

  showError(summaryKey: string, detailKey: string, params?: any): void {
    this.showToast('error', summaryKey, detailKey, params);
  }

  showWarn(summaryKey: string, detailKey: string, params?: any): void {
    this.showToast('warn', summaryKey, detailKey, params);
  }

  showApiError(error: HttpErrorResponse, detailKey: string): void {
    const errors = error.error.errors;
    const errorMessage = Object.keys(errors).map(key => errors[key]).join(', ');
    this.showError(
      'common.error',
      detailKey,
      { message: errorMessage },
    );
  };

  private showToast(severity: string, summaryKey: string, detailKey: string, params?: any): void {
    this.translateService.get([summaryKey, detailKey], params).subscribe(translations => {
      this.messageService.add({
        severity,
        summary: translations[summaryKey],
        detail: translations[detailKey]
      });
    });
  }
}
