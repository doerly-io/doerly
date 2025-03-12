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
import { DatePipe, NgIf } from '@angular/common';
import { ButtonDirective, ButtonIcon } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { Divider } from 'primeng/divider';

import * as sexVariants from '../../constants/sex';
import { Textarea } from 'primeng/textarea';
import { Select } from 'primeng/select';
import { DatePicker } from 'primeng/datepicker';
import { ProfileRequest } from '../../models/requests/ProfileRequest';
import { dateTimeToDateString } from 'utils/dateUtils';
import { ToastHelper } from 'app/@core/helpers/toast.helper';

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
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  originalProfile: any;
  isEdit: boolean = false;
  sexOptions: { label: string; value: number }[] = [];

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
      bio: ['']
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
    const request = this.prepareRequest();
    this.profileService.save(request).subscribe({
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

  private prepareRequest = () => {
    const profileFormData = this.profileForm.value;
    const formattedDate = dateTimeToDateString(profileFormData.dateOfBirth);
    const profileRequest: ProfileRequest = {
      userId: profileFormData.id,
      firstName: profileFormData.firstName,
      lastName: profileFormData.lastName,
      patronymic: profileFormData.patronymic,
      dateOfBirth: formattedDate?.toString() || '',
      sex: profileFormData.sex,
      bio: profileFormData.bio
    };
    return profileRequest;
  }

  toggleEdit(): void {
    if (this.isEdit && this.originalProfile) {
      this.profileForm.patchValue(this.originalProfile);
    }
    this.isEdit = !this.isEdit;
  }

}
