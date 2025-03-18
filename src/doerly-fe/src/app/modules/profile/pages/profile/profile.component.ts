import {
  Component,
  OnInit
} from '@angular/core';
import {
  FormBuilder,
  FormGroup, FormsModule,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ProfileService } from '../../domain/profile.service';
import { Card } from 'primeng/card';
import { InputText } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { DatePipe, NgIf, NgOptimizedImage } from '@angular/common';
import { ButtonDirective, ButtonIcon } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { Divider } from 'primeng/divider';

import * as sexVariants from '../../constants/sex';
import { Textarea } from 'primeng/textarea';
import { Select } from 'primeng/select';
import { DatePicker } from 'primeng/datepicker';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { Dialog } from 'primeng/dialog';

@Component({
  selector: 'app-profile',
  imports: [
    Card,
    ReactiveFormsModule,
    InputText,
    DropdownModule,
    NgIf,
    ButtonDirective,
    TranslatePipe,
    Divider,
    ButtonIcon,
    DatePipe,
    FormsModule,
    Textarea,
    Select,
    DatePicker,
    Dialog,
    NgOptimizedImage,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  originalProfile: any;
  isEdit: boolean = false;
  sexOptions: { label: string; value: number }[] = [];

  isUploadDialogVisible = false;
  previewImage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private profileService: ProfileService,
    private toastHelper: ToastHelper,
  ) {
  }

  ngOnInit(): void {
    this.initForm();
    this.loadProfile();
    this.sexOptions = [
      { label: 'profile.basic.sex.options.male', value: sexVariants.MALE },
      { label: 'profile.basic.sex.options.female', value: sexVariants.FEMALE },
    ];
  }

  initForm(): void {
    this.profileForm = this.formBuilder.group({
      id: [0],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      patronymic: [''],
      dateOfBirth: ['', Validators.required],
      sex: [1, Validators.required],
      bio: [''],
      imageUrl: [''],
    });
  }

  loadProfile(): void {
    this.profileService.load().subscribe({
      next: (profile) => {
        this.originalProfile = profile;
        this.profileForm.patchValue(profile);
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 404) {
          this.toastHelper.showApiError(error, 'profile.message.load.error');
        }
      }
    });
  }

  saveProfile(): void {
    const formValue = this.profileForm.value;
    this.profileService.update({
      userId: formValue.id,
      ...formValue
    }).subscribe({
      next: () => {
        this.toggleEdit();
        this.loadProfile();
        this.toastHelper.showSuccess('common.success', 'profile.message.save.success');
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 400) {
          this.toastHelper.showApiError(error, 'profile.message.save.error');
        }
      }
    });
  }

  toggleEdit(): void {
    if (this.isEdit && this.originalProfile) {
      this.profileForm.patchValue(this.originalProfile);
    }
    this.isEdit = !this.isEdit;
  }

  openUploadDialog() {
    this.isUploadDialogVisible = true;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.previewImage = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  processUpload() {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput.files && fileInput.files.length) {
      this.profileService.uploadImage(fileInput.files[0]).subscribe({
        next: () => {
          this.loadProfile();
          this.toastHelper.showSuccess('common.success', 'profile.message.upload.success');
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.toastHelper.showApiError(error, 'profile.message.upload.error');
          }
        }
      });
    }
    this.isUploadDialogVisible = false;
  }
}
