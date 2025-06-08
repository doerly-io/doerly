import { Injectable } from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { ToastHelper } from 'app/@core/helpers/toast.helper';

@Injectable({ providedIn: 'root' })
export class ErrorHandlerService {
    constructor(private toastHelper: ToastHelper) { }

    handleApiError(error: HttpErrorResponse, defaultMessage: string = 'common.error_occurred') {
        // Helper to extract messages from validation error dictionary
        const extractValidationMessages = (errObj: any): string => {
            if (typeof errObj === 'object' && errObj !== null) {
                return Object.values(errObj)
                    .map(val => Array.isArray(val) ? val.join(', ') : val)
                    .join(' ');
            }
            return '';
        };

        // 400: Validation or business logic error
        if (error.status === 400) {
            // ASP.NET Core/FluentValidation style: errors dictionary inside error.error.errors
            if (error.error && typeof error.error === 'object' && error.error.errors) {
                const msg = extractValidationMessages(error.error.errors);
                this.toastHelper.showError('common.error', msg || defaultMessage);
                return;
            }
            // Validation dictionary (e.g. { Name: ["..."], ... })
            if (error.error && typeof error.error === 'object' && !error.error.errorMessage) {
                const msg = extractValidationMessages(error.error);
                this.toastHelper.showError('common.error', msg || defaultMessage);
                return;
            }
            // Business logic error
            if (error.error?.errorMessage) {
                this.toastHelper.showError('common.error', error.error.errorMessage);
                return;
            }
            // Plain string error
            if (typeof error.error === 'string') {
                this.toastHelper.showError('common.error', error.error);
                return;
            }
            this.toastHelper.showError('common.error', defaultMessage);
            return;
        }

        // 401: Unauthorized, show message if present
        if (error.status === 401) {
            if (error.error?.errorMessage) {
                this.toastHelper.showError('common.error', error.error.errorMessage);
                return;
            }
            if (typeof error.error === 'string') {
                this.toastHelper.showError('common.error', error.error);
                return;
            }
            this.toastHelper.showError('common.error', 'common.unauthorized');
            return;
        }

        // 500: Internal server error
        if (error.status === 500) {
            this.toastHelper.showError('common.error', 'common.error_occurred');
            return;
        }

        // Other errors: try to show message, else generic
        if (error.error?.errorMessage) {
            this.toastHelper.showError('common.error', error.error.errorMessage);
            return;
        }
        if (typeof error.error === 'string') {
            this.toastHelper.showError('common.error', error.error);
            return;
        }
        this.toastHelper.showError('common.error', defaultMessage);
    }
}