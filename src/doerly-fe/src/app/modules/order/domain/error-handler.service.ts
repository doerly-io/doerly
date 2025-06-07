import { Injectable } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { ToastHelper } from 'app/@core/helpers/toast.helper';

@Injectable({ providedIn: 'root' })
export class ErrorHandlerService {
    constructor(private toastHelper: ToastHelper) { }

    handleApiError(error: HttpErrorResponse, defaultMessage: string = 'common.error_occurred') {
        if (error.status === 400) {
            this.toastHelper.showError('common.error', error.error?.errorMessage || defaultMessage);
        } else {
            this.toastHelper.showError('common.error', defaultMessage);
        }
    }
}