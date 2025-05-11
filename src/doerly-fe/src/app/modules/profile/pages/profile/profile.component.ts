import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,
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
import { Observable, catchError, tap } from 'rxjs';

import * as sexVariants from '../../constants/sex';
import { Textarea } from 'primeng/textarea';
import { Select } from 'primeng/select';
import { DatePicker } from 'primeng/datepicker';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { Dialog } from 'primeng/dialog';
import { Tooltip } from 'primeng/tooltip';
import { PdfService } from 'app/@core/services/pdf.service';
import { ProfileResponse } from '../../models/responses/ProfileResponse';

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
    Tooltip
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  formProfile!: FormGroup;
  initialProfile: any;
  isEdit: boolean = false;
  sexOptions: { label: string; value: number }[] = [];

  isImageUploadDialogVisible = false;
  previewImage: string | null = null;
  isImageAvailable = false;

  isCVUploadDialogVisible = false;
  previewCV: File | null = null;
  isCVPreviewDialogVisible = false;
  isCVAvailable = false;
  @ViewChild('pdfContainer') pdfContainer!: ElementRef;

  constructor(
    private formBuilder: FormBuilder,
    private profileService: ProfileService,
    private toastHelper: ToastHelper,
    public pdfService: PdfService
  ) {}

  ngOnInit(): void {
    this.onInitForm();
    this.onLoadProfile();
    this.sexOptions = [
      { label: 'profile.basic.sex.options.male', value: sexVariants.MALE },
      { label: 'profile.basic.sex.options.female', value: sexVariants.FEMALE },
    ];
  }

  onInitForm(): void {
    this.formProfile = this.formBuilder.group({
      id: [0],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      patronymic: [''],
      dateOfBirth: ['', Validators.required],
      sex: [1, Validators.required],
      bio: [''],
      imageUrl: [''],
      cvUrl: ['']
    });
  }

  // TODO: Extract this to a basic service
  private handleError(error: HttpErrorResponse, errorKey: string): void {
    if (error.status === 400 || error.status === 404) {
      this.toastHelper.showApiError(error, errorKey);
    }
  }

  // TODO: Extract this to a basic service
  private handleProfileOperation<T>(
    operation: Observable<T>,
    successMessage: string,
    errorMessage: string,
    onSuccess?: (result: T) => void
  ): void {
    operation.pipe(
      tap((result) => {
        if (successMessage) this.toastHelper.showSuccess('common.success', successMessage);
        if (onSuccess) {
          onSuccess(result);
        }
      }),
      catchError((error: HttpErrorResponse) => {
        this.handleError(error, errorMessage);
        throw error;
      })
    ).subscribe();
  }

  onLoadProfile(
    loadCV: boolean = false
  ): void {
    this.handleProfileOperation(
      this.profileService.load(),
      '',
      'profile.message.load.error',
      (profile) => {
        this.updateProfileState(profile.value!!)
        if (loadCV) this.onPreviewCV();
      }
    );
  }

  private updateProfileState(profileValue: ProfileResponse): void {
    this.initialProfile = profileValue;
    this.formProfile.patchValue(profileValue);
    this.isImageAvailable = !!profileValue.imageUrl;
    this.isCVAvailable = !!profileValue.cvUrl;
  }

  onSaveProfile(): void {
    const formValue = this.formProfile.value;
    this.handleProfileOperation(
      this.profileService.update({
        userId: formValue.id,
        ...formValue
      }),
      'profile.message.save.success',
      'profile.message.save.error',
      () => {
        this.onEditProfile();
        this.onLoadProfile();
      }
    );
  }

  onEditProfile(): void {
    if (this.isEdit && this.initialProfile) {
      this.formProfile.patchValue(this.initialProfile);
    }
    this.isEdit = !this.isEdit;
  }

  // region Image operations
  onOpenImageUploadDialog() {
    this.isImageUploadDialogVisible = true;
  }

  onImageFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.previewImage = e.target?.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  onImageUpload() {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput.files && fileInput.files.length) {
      this.handleProfileOperation(
        this.profileService.uploadImage(fileInput.files[0]),
        'profile.message.upload.success',
        'profile.message.upload.error',
        () => {
          this.onLoadProfile();
          this.isImageUploadDialogVisible = false;
        }
      );
    }
  }

  onImageDelete() {
    this.handleProfileOperation(
      this.profileService.deleteImage(),
      'profile.message.image.delete.success',
      'profile.message.image.delete.error',
      () => {
        this.onLoadProfile();
        this.isImageAvailable = false;
        this.isImageUploadDialogVisible = false;
        this.previewImage = null;
      }
    );
  }
  // endregion

  // region CV operations
  onOpenUploadCVDialog() {
    this.isCVUploadDialogVisible = true;
  }

  onCVFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.previewCV = file;
    }
  }

  onUploadCV() {
    if (this.previewCV) {
      this.handleProfileOperation(
        this.profileService.uploadCV(this.previewCV),
        'profile.message.cv.upload.success',
        'profile.message.cv.upload.error',
        () => {
          this.isCVUploadDialogVisible = false;
          this.previewCV = null;
          this.onLoadProfile(true);
        }
      );
    }
  }

  onDeleteCV() {
    this.handleProfileOperation(
      this.profileService.deleteCV(),
      'profile.message.cv.delete.success',
      'profile.message.cv.delete.error',
      () => {
        this.onLoadProfile();
        this.isCVAvailable = false;
        this.isCVPreviewDialogVisible = false;
        this.pdfService.reset();
      }
    );
  }

  onDownloadCV() {
    const cvUrl = this.formProfile.get('cvUrl')?.value;
    if (cvUrl) {
      window.open(cvUrl, '_blank');
    }
  }

  onPreviewCV() {
    this.isCVPreviewDialogVisible = true;
    const cvUrl = this.formProfile.get('cvUrl')?.value;
    if (cvUrl) {
      this.pdfService.loadPdf(cvUrl)
        .then(() => this.pdfService.renderPage(
          this.pdfService.getCurrentPage(),
          this.pdfContainer.nativeElement
        ))
        .catch(error => {
          this.toastHelper.showError('common.error', error);
        });
    }
  }

  onCVNextPage() {
    if (this.pdfContainer) {
      this.pdfService.nextPage(this.pdfContainer.nativeElement)
        .catch(error => {
          this.toastHelper.showError('common.error', error);
        });
    }
  }

  onCVPreviousPage() {
    if (this.pdfContainer) {
      this.pdfService.previousPage(this.pdfContainer.nativeElement)
        .catch(error => {
          this.toastHelper.showError('common.error', error);
        });
    }
  }
  // endregion
}
