import {Component, Input, ViewEncapsulation} from '@angular/core';
import {AbstractControl, FormControl, ReactiveFormsModule} from '@angular/forms';
import {PasswordDirective} from 'primeng/password';
import {NgIf} from '@angular/common';
import {InputText} from 'primeng/inputtext';

@Component({
  selector: 'doerly-password-input',
  templateUrl: './password-input.component.html',
  imports: [
    PasswordDirective,
    ReactiveFormsModule,
    NgIf,
    InputText
  ],
  styleUrls: ['./password-input.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PasswordInputComponent {
  private _inputControl!: FormControl;

  @Input()
  set inputControl(control: AbstractControl) {
    this._inputControl = control as FormControl;
  }
  get inputControl(): FormControl {
    return this._inputControl;
  }

  @Input() elementId: string = 'password';
  @Input() label: string = '';
  @Input() placeholder: string = '';
  isPasswordVisible: boolean = false;

  togglePasswordVisibility() {
    this.isPasswordVisible = !this.isPasswordVisible;
  }


}
