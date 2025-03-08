import {AbstractControl, FormGroup} from '@angular/forms';

export function setServerErrors(form: FormGroup, errors: { [key: string]: string[] }) {
  Object.keys(errors).forEach(field => {
    const formControl = form.get(field);
    if (formControl) {
      formControl.setErrors({serverError: errors[field][0]});
    }
  });
}

export function isInvalid(formGroup: FormGroup, controlName: string): boolean {
  const control = formGroup.get(controlName);
  return control?.touched && control?.invalid || false;
}

export function getError(formGroup: FormGroup, controlName: string, error: string, errorUIText: string): string {
  return formGroup.get(controlName)?.hasError(error) ? errorUIText : '';
}

export function getServersideError(formGroup: FormGroup, controlName: string): string {
  return formGroup.get(controlName)?.errors?.['serverError'] || '';
}


