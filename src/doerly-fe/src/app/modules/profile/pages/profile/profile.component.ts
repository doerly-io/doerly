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
import { DropdownModule, DropdownLazyLoadEvent } from 'primeng/dropdown';
import { DatePipe, DecimalPipe, NgForOf, NgIf, NgOptimizedImage } from '@angular/common';
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
import { AddressSelectComponent } from 'app/@shared/components/address-select/address-select.component';
import { ProfileRequest } from '../../models/requests/ProfileRequest';
import { LanguageProficiencyDto } from '../../models/responses/LanguageProficiencyDto';
import { LanguageDto } from '../../models/responses/LanguageDto';
import { ELanguageProficiencyLevel } from '../../models/enums/ELanguageProficiencyLevel';
import { LanguagesQueryDto } from '../../domain/profile.service';

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
    DecimalPipe,
    FormsModule,
    Textarea,
    Select,
    DatePicker,
    Dialog,
    NgOptimizedImage,
    Tooltip,
    AddressSelectComponent,
    NgForOf
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  formProfile!: FormGroup;
  languageProficiencyForm!: FormGroup;
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

  isLanguageProficiencyDialogVisible = false;
  languageProficiencies: LanguageProficiencyDto[] = [];
  availableLanguages: LanguageDto[] = [];
  proficiencyLevels: { label: string; value: ELanguageProficiencyLevel }[] = [];
  editingProficiency: LanguageProficiencyDto | null = null;

  languagesQuery: LanguagesQueryDto = {
    number: 1,
    size: 20,
    name: ''
  };
  totalLanguages: number = 0;

  private searchTimeout: any;
  private isSearching: boolean = false;

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
    this.initLanguageProficiencyForm();
    this.initProficiencyLevels();
    this.loadAvailableLanguages();
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
      cvUrl: [''],
      address: this.formBuilder.group({
        cityId: [null, Validators.required],
        cityName: [''],
        regionId: [null, Validators.required],
        regionName: ['']
      })
    });
  }

  private initLanguageProficiencyForm(): void {
    this.languageProficiencyForm = this.formBuilder.group({
      languageId: [null, Validators.required],
      level: [null, Validators.required]
    });
  }

  private initProficiencyLevels(): void {
    this.proficiencyLevels = [
      { label: 'profile.professional.languages.level.beginner', value: ELanguageProficiencyLevel.Beginner },
      { label: 'profile.professional.languages.level.elementary', value: ELanguageProficiencyLevel.Elementary },
      { label: 'profile.professional.languages.level.intermediate', value: ELanguageProficiencyLevel.Intermediate },
      { label: 'profile.professional.languages.level.upperIntermediate', value: ELanguageProficiencyLevel.UpperIntermediate },
      { label: 'profile.professional.languages.level.advanced', value: ELanguageProficiencyLevel.Advanced },
      { label: 'profile.professional.languages.level.proficient', value: ELanguageProficiencyLevel.Proficient },
      { label: 'profile.professional.languages.level.native', value: ELanguageProficiencyLevel.Native }
    ];
  }

  private loadAvailableLanguages(): void {
    this.languagesQuery.number = Math.max(1, this.languagesQuery.number);
    this.profileService.getAvailableLanguages(this.languagesQuery).subscribe({
      next: (response) => {
        if (this.languagesQuery.number === 1) {
          this.availableLanguages = response.list;
        } else {
          this.availableLanguages = [...this.availableLanguages, ...response.list];
        }
        this.totalLanguages = response.totalSize;
        this.isSearching = false;
      },
      error: (error) => {
        this.handleError(error, 'profile.professional.languages.load.error');
        this.isSearching = false;
      }
    });
  }

  onLanguageSearch(event: { filter: string }): void {
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout);
    }
    this.searchTimeout = setTimeout(() => {
      this.isSearching = true;
      this.languagesQuery.name = event.filter;
      this.languagesQuery.number = 1;
      this.availableLanguages = [];
      this.loadAvailableLanguages();
    }, 300);
  }

  onLanguagePageChange(event: DropdownLazyLoadEvent): void {
    if (this.isSearching) {
      return;
    }

    const loadedItems = this.availableLanguages.length;
    const totalItems = this.totalLanguages;
    const scrollPosition = event.first;
    const itemsToLoad = this.languagesQuery.size;

    if (loadedItems < totalItems && scrollPosition + itemsToLoad >= loadedItems) {
      this.languagesQuery.number = Math.floor(loadedItems / this.languagesQuery.size) + 1;
      this.loadAvailableLanguages();
    }
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
    this.languageProficiencies = profileValue.languageProficiencies || [];
  }

  onSaveProfile(): void {
    const formValue = this.formProfile.value;
    const request : ProfileRequest = {
      userId: formValue.id,
      firstName: formValue.firstName,
      lastName: formValue.lastName,
      patronymic: formValue.patronymic,
      dateOfBirth: formValue.dateOfBirth,
      sex: formValue.sex,
      bio: formValue.bio,
      cityId: formValue.address.cityId
    }
    this.handleProfileOperation(
      this.profileService.update(request),
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

  onCVZoomIn() {
    if (this.pdfContainer) {
      this.pdfService.zoomIn();
      this.pdfService.renderPage(
        this.pdfService.getCurrentPage(),
        this.pdfContainer.nativeElement
      ).catch(error => {
        this.toastHelper.showError('common.error', error);
      });
    }
  }

  onCVZoomOut() {
    if (this.pdfContainer) {
      this.pdfService.zoomOut();
      this.pdfService.renderPage(
        this.pdfService.getCurrentPage(),
        this.pdfContainer.nativeElement
      ).catch(error => {
        this.toastHelper.showError('common.error', error);
      });
    }
  }

  getZoomLevel(): number {
    return this.pdfService.getZoomLevel();
  }
  // endregion

  onAddressChange(address: { cityId: number }): void {
    this.formProfile.patchValue({
      address: {
        cityId: address.cityId
      }
    });
  }

  // region Language Proficiency methods
  getTranslatedLevel(level: ELanguageProficiencyLevel): string {
    const levelConfig = this.proficiencyLevels.find(l => l.value === level);
    return levelConfig ? levelConfig.label : '';
  }

  onAddLanguageProficiency(): void {
    this.editingProficiency = null;
    this.languageProficiencyForm.reset();
    this.availableLanguages = [];
    this.isLanguageProficiencyDialogVisible = true;
  }

  onEditLanguageProficiency(proficiency: LanguageProficiencyDto): void {
    this.editingProficiency = proficiency;
    this.languageProficiencyForm.patchValue({
      languageId: proficiency.language?.id,
      level: proficiency.level
    });
    if (proficiency.language && !this.availableLanguages.some(l => l.id === proficiency.language.id)) {
      this.availableLanguages = [proficiency.language, ...this.availableLanguages];
    }
    this.isLanguageProficiencyDialogVisible = true;
  }

  onDeleteLanguageProficiency(proficiency: LanguageProficiencyDto): void {
    this.handleProfileOperation(
      this.profileService.deleteLanguageProficiency(proficiency.id),
      'profile.professional.languages.delete.success',
      'profile.professional.languages.delete.error',
      () => {
        this.languageProficiencies = this.languageProficiencies.filter(p => p.id !== proficiency.id);
      }
    );
  }

  onCancelLanguageProficiency(): void {
    this.isLanguageProficiencyDialogVisible = false;
    this.editingProficiency = null;
    this.languageProficiencyForm.reset();
    this.languagesQuery.name = '';
    this.availableLanguages = [];
  }

  onSaveLanguageProficiency(): void {
    if (this.languageProficiencyForm.invalid) return;

    const formValue = this.languageProficiencyForm.value;
    const operation = this.editingProficiency
      ? this.profileService.updateLanguageProficiency(this.editingProficiency.id, formValue)
      : this.profileService.addLanguageProficiency(formValue);

    this.handleProfileOperation(
      operation,
      this.editingProficiency
        ? 'profile.professional.languages.update.success'
        : 'profile.professional.languages.add.success',
      this.editingProficiency
        ? 'profile.professional.languages.update.error'
        : 'profile.professional.languages.add.error',
      () => {
        this.onCancelLanguageProficiency();
        this.onLoadProfile();
      }
    );
  }
  // endregion
}
