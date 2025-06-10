import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,
  inject
} from '@angular/core';
import {
  FormBuilder,
  FormGroup, FormsModule,
  ReactiveFormsModule,
  Validators,
  FormControl
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
import { TreeSelect } from 'primeng/treeselect';
import { TreeNode } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';
import { CheckboxModule } from 'primeng/checkbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { MultiSelectModule } from 'primeng/multiselect';
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
import { CategoryService } from 'app/@core/services/category.service';
import { ICategory } from 'app/@core/models/category/category.model';
import { CompetenceDto } from '../../models/responses/CompetenceDto';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import {PaymentHistoryComponent} from 'app/modules/profile/components/payment-history/payment-history.component';
import {LanguagesQueryDto} from '../../models/requests/LanguagesQuery';
import {Toast} from 'primeng/toast';
import { MessageService } from 'primeng/api';
import {
  SendMessageModalComponent
} from '../../../communication/pages/modals/send-message-modal/send-message-modal.component';
import { IService, ICreateServiceRequest, IUpdateServiceRequest, IFilterValueRequest } from '../../../catalog/models/service.model';
import { InputNumberModule } from 'primeng/inputnumber';
import { CatalogService } from '../../../catalog/services/catalog.service';
import { IFilter, EFilterType } from '../../../catalog/models/filter.model';

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
    NgForOf,
    PaymentHistoryComponent,
    InputNumberModule,
    Toast,
    SendMessageModalComponent,
    CheckboxModule,
    RadioButtonModule,
    MultiSelectModule
  ],
  providers: [MessageService],
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

  categories: ICategory[] = [];

  private searchTimeout: any;
  private isSearching: boolean = false;

  isCompetenceDialogVisible = false;
  competences: CompetenceDto[] = [];
  competenceForm!: FormGroup;
  editingCompetence: CompetenceDto | null = null;
  categoryTreeNodes: TreeNode[] = [];

  private jwtTokenHelper = inject(JwtTokenHelper);

  isViewingOtherProfile: boolean = false;
  viewedUserId: number | null = null;

  services: IService[] = [];
  isServiceDialogVisible = false;
  editingService: IService | null = null;
  serviceForm: FormGroup;
  userId: number = 0;

  showMessageDialog = false;

  private readonly formBuilder: FormBuilder = inject(FormBuilder);
  private readonly profileService: ProfileService = inject(ProfileService);
  private readonly categoryService: CategoryService = inject(CategoryService);
  private readonly toastHelper: ToastHelper = inject(ToastHelper);
  private readonly route: ActivatedRoute = inject(ActivatedRoute);
  public pdfService: PdfService = inject(PdfService);
  private readonly catalogService: CatalogService = inject(CatalogService);

  @ViewChild('competenceDialog') competenceDialog: any;
  @ViewChild('serviceDialog') serviceDialog: any;

  categoryFilters: IFilter[] = [];
  private filterValuesMap: { [key: number]: string[] } = {};

  constructor(
    private fb: FormBuilder,
  ) {
    this.serviceForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      categoryId: [null, Validators.required],
      price: [null, Validators.required],
      filterValues: this.fb.group({})
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const userId = params['userId'];
      if (userId) {
        this.viewedUserId = parseInt(userId);
        const tokenUserId = this.jwtTokenHelper.getUserInfo()?.id;
        this.isViewingOtherProfile = this.viewedUserId !== tokenUserId;
        this.isEdit = false; // Disable editing for other profiles
        this.userId = this.viewedUserId;
      } else {
        this.viewedUserId = null;
        this.isViewingOtherProfile = false;
        this.userId = this.jwtTokenHelper.getUserInfo()?.id || 0;
      }
      this.onInitForm();
      this.onLoadProfile();
      this.loadServices();
    });

    this.sexOptions = [
      { label: 'profile.basic.sex.options.male', value: sexVariants.MALE },
      { label: 'profile.basic.sex.options.female', value: sexVariants.FEMALE },
    ];
    this.initLanguageProficiencyForm();
    this.initProficiencyLevels();
    this.loadAvailableLanguages();
    this.loadCategories();
    this.initCompetenceForm();
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

  private loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (response) => {
        this.categories = response.value || [];
        this.categoryTreeNodes = this.convertCategoriesToTreeNodes(this.categories);
        if (!Array.isArray(this.categoryTreeNodes)) {
          this.categoryTreeNodes = [];
        }
      },
      error: (error) => {
        this.handleError(error, 'profile.professional.competences.categories.load.error');
        this.categoryTreeNodes = [];
      }
    });
  }

  private convertCategoriesToTreeNodes(categories: ICategory[]): any[] {
    if (!Array.isArray(categories)) {
      return [];
    }
    
    const flattenCategories = (cats: ICategory[], parentPath: string = ''): any[] => {
      return cats.reduce((acc: any[], category) => {
        const currentPath = parentPath ? `${parentPath} > ${category.name}` : category.name;
        acc.push({
          key: category.id.toString(),
          label: currentPath,
          data: category
        });
        
        if (category.children && category.children.length > 0) {
          acc.push(...flattenCategories(category.children, currentPath));
        }
        
        return acc;
      }, []);
    };

    return flattenCategories(categories);
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
    const operation = this.isViewingOtherProfile && this.viewedUserId
      ? this.profileService.loadById(this.viewedUserId)
      : this.profileService.load();

    this.handleProfileOperation(
      operation,
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
    this.competences = profileValue.competences || [];
  }

  onSaveProfile(): void {
    const formValue = this.formProfile.value;
    const request : ProfileRequest = {
      userId: this.jwtTokenHelper.getUserInfo()?.id || 0,
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

  onAddressChange(address: { cityId: number, regionId?: number }) {
    this.formProfile.get('address')?.patchValue({
      cityId: address.cityId,
      regionId: address.regionId ?? null
    });
  }

  get addressFormGroup(): FormGroup {
    return this.formProfile.get('address') as FormGroup;
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

  private initCompetenceForm(): void {
    this.competenceForm = this.formBuilder.group({
      categoryId: [null, Validators.required],
      categoryName: [null, Validators.required]
    });
  }

  onAddCompetence(): void {
    this.editingCompetence = null;
    this.competenceForm.reset();
    this.isCompetenceDialogVisible = true;
  }

  onDeleteCompetence(competence: CompetenceDto): void {
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    this.handleProfileOperation(
      this.profileService.deleteCompetence(userId!, competence.id),
      'profile.professional.competences.delete.success',
      'profile.professional.competences.delete.error',
      () => {
        this.competences = this.competences.filter(c => c.id !== competence.id);
      }
    );
  }

  onCancelCompetence(): void {
    this.isCompetenceDialogVisible = false;
    this.editingCompetence = null;
    this.competenceForm.reset();
  }

  onSaveCompetence(): void {
    if (this.competenceForm.invalid) return;

    const formValue = this.competenceForm.value;
    const userId = this.jwtTokenHelper.getUserInfo()?.id;
    const request = {
      categoryId: parseInt(formValue.categoryId),
      categoryName: formValue.categoryName
    };
    const operation = this.profileService.addCompetence(userId!, request);

    this.handleProfileOperation(
      operation,
      'profile.professional.competences.add.success',
      'profile.professional.competences.add.error',
      () => {
        this.onCancelCompetence();
        this.onLoadProfile();
      }
    );
  }

  onCategorySelect(event: any): void {
    if (event.value) {
      const selectedCategory = this.categoryTreeNodes.find(cat => cat.key === event.value);
      if (selectedCategory) {
        this.competenceForm.patchValue({
          categoryId: selectedCategory.key,
          categoryName: selectedCategory.label
        });
      }
    }
  }

  onAddService() {
    this.editingService = null;
    this.serviceForm.reset();
    // Reset filter values and category filters
    this.filterValuesMap = {};
    this.categoryFilters = [];
    // Clear filter form controls
    const filterValuesGroup = this.serviceForm.get('filterValues') as FormGroup;
    if (filterValuesGroup) {
      Object.keys(filterValuesGroup.controls).forEach(key => {
        filterValuesGroup.removeControl(key);
      });
    }
    this.isServiceDialogVisible = true;
  }

  onEditService(service: IService) {
    this.editingService = service;
    this.serviceForm.patchValue({
      name: service.name,
      description: service.description,
      categoryId: service.categoryId?.toString(),
      price: service.price,
    });
    
    // Load category filters and set values
    if (service.categoryId) {
      this.loadCategoryFilters(service.categoryId);
    }
    
    // Set filter values from service
    if (service.filterValues) {
      service.filterValues.forEach(filterValue => {
        const filterId = filterValue.filterId;
        if (filterValue.value) {
          this.filterValuesMap[filterId] = [filterValue.value];
        }
      });
    }
    
    this.isServiceDialogVisible = true;
  }

  onDeleteService(service: IService) {
    this.handleProfileOperation(
      this.catalogService.deleteService(service.id),
      'profile.professional.services.delete.success',
      'profile.professional.services.delete.error',
      () => {
        this.services = this.services.filter(s => s.id !== service.id);
      }
    );
  }

  onCancelService() {
    this.isServiceDialogVisible = false;
    this.editingService = null;
    this.serviceForm.reset();
    // Reset filter values and category filters
    this.filterValuesMap = {};
    this.categoryFilters = [];
    // Clear filter form controls
    const filterValuesGroup = this.serviceForm.get('filterValues') as FormGroup;
    if (filterValuesGroup) {
      Object.keys(filterValuesGroup.controls).forEach(key => {
        filterValuesGroup.removeControl(key);
      });
    }
  }

  onServiceCategorySelect(event: any): void {
    if (event.value) {
      const selectedCategory = this.categoryTreeNodes.find(cat => cat.key === event.value);
      if (selectedCategory) {
        this.serviceForm.patchValue({
          categoryId: selectedCategory.key
        });
        this.loadCategoryFilters(Number(selectedCategory.key));
        // Reset filter values when category changes
        this.filterValuesMap = {};
      }
    }
  }

  private loadCategoryFilters(categoryId: number): void {
    this.catalogService.getFiltersByCategoryId(categoryId).subscribe({
      next: (response) => {
        if (response.isSuccess && response.value) {
          this.categoryFilters = response.value;
          this.updateFilterFormControls();
        }
      },
      error: (error) => {
        this.toastHelper.showError('common.error', 'profile.professional.services.filters.load.error');
      }
    });
  }

  private updateFilterFormControls(): void {
    const filterValuesGroup = this.serviceForm.get('filterValues') as FormGroup;
    
    // Clear existing controls
    Object.keys(filterValuesGroup.controls).forEach(key => {
      filterValuesGroup.removeControl(key);
    });

    // Add controls for each filter
    this.categoryFilters.forEach(filter => {
      if (filter.type === 4) {
        // For boolean checkbox - initialize with false
        filterValuesGroup.addControl(filter.id.toString(), new FormControl(false));
      } else {
        filterValuesGroup.addControl(filter.id.toString(), new FormControl([]));
      }
    });

    // If editing existing service, set values
    if (this.editingService) {
      this.editingService.filterValues.forEach(filterValue => {
        const key = filterValue.filterId.toString();
        const filter = this.categoryFilters.find(f => f.id === filterValue.filterId);
        
        if (filter) {
          if (filter.type === 1 || filter.type === 2) {
            // For checkbox and dropdown - add to array
            const currentValues = filterValuesGroup.get(key)?.value || [];
            filterValuesGroup.get(key)?.setValue([...currentValues, filterValue.value]);
            this.filterValuesMap[filter.id] = [...(this.filterValuesMap[filter.id] || []), filterValue.value];
          } else if (filter.type === 4) {
            // For boolean checkbox - convert string to boolean
            const boolValue = filterValue.value.toLowerCase() === 'true';
            filterValuesGroup.get(key)?.setValue(boolValue);
            this.filterValuesMap[filter.id] = [filterValue.value];
          } else {
            // For price - set single value
            filterValuesGroup.get(key)?.setValue(filterValue.value);
            this.filterValuesMap[filter.id] = [filterValue.value];
          }
        }
      });
    }
  }

  onFilterValueChange(event: any, filterId: number): void {
    const filter = this.categoryFilters.find(f => f.id === filterId);
    if (!filter) return;

    const control = this.serviceForm.get('filterValues')?.get(filterId.toString());
    if (!control) return;

    if (filter.type === 1 || filter.type === 2) {
      // For checkbox and dropdown - update array
      const values = Array.isArray(event.value) ? event.value : [];
      this.filterValuesMap[filterId] = values;
    } else if (filter.type === 3) {
      // For price - ensure it's a valid number
      const numValue = Number(event.value);
      if (!isNaN(numValue)) {
        this.filterValuesMap[filterId] = [numValue.toString()];
      }
    } else if (filter.type === 4) {
      // For boolean checkbox - convert to string
      const boolValue = event.checked;
      this.filterValuesMap[filterId] = [boolValue.toString()];
    }
  }

  onAddCheckboxValue(filterId: number, value: string): void {
    if (!value.trim()) return;
    if (!this.filterValuesMap[filterId]) {
      this.filterValuesMap[filterId] = [];
    }
    if (!this.filterValuesMap[filterId].includes(value)) {
      this.filterValuesMap[filterId].push(value);
      const control = this.serviceForm.get('filterValues')?.get(filterId.toString());
      if (control) {
        control.setValue([...this.filterValuesMap[filterId]]);
      }
    }
  }

  onRemoveCheckboxValue(filterId: number, value: string): void {
    if (this.filterValuesMap[filterId]) {
      this.filterValuesMap[filterId] = this.filterValuesMap[filterId].filter(v => v !== value);
      const control = this.serviceForm.get('filterValues')?.get(filterId.toString());
      if (control) {
        control.setValue([...this.filterValuesMap[filterId]]);
      }
    }
  }

  onAddDropdownValue(filterId: number, value: string): void {
    if (!value.trim()) return;
    if (!this.filterValuesMap[filterId]) {
      this.filterValuesMap[filterId] = [];
    }
    if (!this.filterValuesMap[filterId].includes(value)) {
      this.filterValuesMap[filterId].push(value);
      const control = this.serviceForm.get('filterValues')?.get(filterId.toString());
      if (control) {
        control.setValue([...this.filterValuesMap[filterId]]);
      }
    }
  }

  onRemoveDropdownValue(filterId: number, value: string): void {
    if (this.filterValuesMap[filterId]) {
      this.filterValuesMap[filterId] = this.filterValuesMap[filterId].filter(v => v !== value);
      const control = this.serviceForm.get('filterValues')?.get(filterId.toString());
      if (control) {
        control.setValue([...this.filterValuesMap[filterId]]);
      }
    }
  }

  onAddRadioValue(filterId: number, value: string): void {
    if (!value.trim()) return;
    this.filterValuesMap[filterId] = [value];
    const control = this.serviceForm.get('filterValues')?.get(filterId.toString());
    if (control) {
      control.setValue(value);
    }
  }

  onRemoveRadioValue(filterId: number, value: string): void {
    if (this.filterValuesMap[filterId]) {
      this.filterValuesMap[filterId] = [];
      const control = this.serviceForm.get('filterValues')?.get(filterId.toString());
      if (control) {
        control.setValue('');
      }
    }
  }

  onSaveService() {
    if (this.serviceForm.valid) {
      const formValue = this.serviceForm.value;
      const filterValues: IFilterValueRequest[] = [];
      
      // Convert filter values to the required format
      Object.entries(formValue.filterValues).forEach(([key, value]) => {
        const filter = this.categoryFilters.find(f => f.id.toString() === key);
        if (filter && value !== null && value !== undefined) {
          if (filter.type === 1 || filter.type === 2) {
            // For checkbox and dropdown - send each value separately
            const values = Array.isArray(value) ? value : [];
            values.forEach(val => {
              filterValues.push({
                filterId: parseInt(key),
                value: val
              });
            });
          } else if (filter.type === 3) {
            // For price - ensure it's a valid number
            const numValue = Number(value);
            if (!isNaN(numValue)) {
              filterValues.push({
                filterId: parseInt(key),
                value: numValue.toString()
              });
            }
          } else if (filter.type === 4) {
            // For boolean checkbox - convert to string
            filterValues.push({
              filterId: parseInt(key),
              value: value.toString()
            });
          }
        }
      });

      if (this.editingService) {
        // Update existing service
        const updateRequest: IUpdateServiceRequest = {
          name: formValue.name,
          description: formValue.description || '',
          categoryId: Number(formValue.categoryId),
          price: formValue.price || 0,
          isEnabled: this.editingService.isEnabled,
          filterValues: filterValues
        };

        console.log('Update Service Request:', {
          ...updateRequest,
          filterValues: JSON.stringify(filterValues, null, 2)
        });

        this.catalogService.updateService(this.editingService.id, updateRequest).subscribe({
          next: (response) => {
            if (response.isSuccess && response.value) {
              this.toastHelper.showSuccess('common.success', 'profile.professional.services.update.success');
              this.loadServices();
              this.isServiceDialogVisible = false;
              this.editingService = null;
              this.serviceForm.reset();
            } else {
              this.toastHelper.showError('common.error', 'profile.professional.services.update.error');
            }
          },
          error: (error: Error) => {
            this.toastHelper.showError('common.error', 'profile.professional.services.update.error');
          }
        });
      } else {
        // Create new service
        const createRequest: ICreateServiceRequest = {
          name: formValue.name,
          description: formValue.description || '',
          categoryId: Number(formValue.categoryId),
          userId: this.jwtTokenHelper.getUserInfo()?.id || 0,
          price: formValue.price || 0,
          filterValues: filterValues
        };

        console.log('Create Service Request:', {
          ...createRequest,
          filterValues: JSON.stringify(filterValues, null, 2)
        });

        this.catalogService.createService(createRequest).subscribe({
          next: (response) => {
            if (response.isSuccess && response.value) {
              this.toastHelper.showSuccess('common.success', 'profile.professional.services.create.success');
              this.loadServices();
              this.isServiceDialogVisible = false;
              this.serviceForm.reset();
            } else {
              this.toastHelper.showError('common.error', 'profile.professional.services.create.error');
            }
          },
          error: (error: Error) => {
            this.toastHelper.showError('common.error', 'profile.professional.services.create.error');
          }
        });
      }
    }
  }

  loadServices() {
    const userId = this.isViewingOtherProfile ? this.viewedUserId : this.jwtTokenHelper.getUserInfo()?.id;
    if (!userId) return;

    console.log('Loading services for user:', userId);
    this.catalogService.getServicesByUserId(userId).subscribe({
      next: (response) => {
        console.log('Load services response:', response);
        if (response.isSuccess && response.value) {
          this.services = response.value;
        }
      },
      error: (error: Error) => {
        console.error('Error loading services:', error);
        this.toastHelper.showError('common.error', 'profile.professional.services.load.error');
      }
    });
  }

  get isAuthenticated(): boolean {
    return this.jwtTokenHelper.isLoggedIn();
  }

  get isOwnProfile(): boolean {
    return !this.isViewingOtherProfile;
  }

  // Add these methods to handle filter values
  getFilterValues(filterId: number): string[] {
    return this.filterValuesMap[filterId] || [];
  }
}
