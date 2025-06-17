import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AddressService } from 'app/@core/services/address.service';
import { Region } from 'app/@core/models/address/region.model';
import { City } from 'app/@core/models/address/city.model';
import { DropdownModule } from 'primeng/dropdown';
import { TranslatePipe } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { BaseApiResponse } from 'app/@core/models/base-api-response';

@Component({
  selector: 'app-address-select',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DropdownModule,
    TranslatePipe
  ],
  template: `
    <div [formGroup]="form" class="d-flex gap-3 flex-column flex-xl-row">
      <div class="responsive-dropdown col">
        <p-dropdown
          [options]="regions"
          formControlName="regionId"
          optionLabel="name"
          optionValue="id"
          [placeholder]="'address.region.placeholder' | translate"
          (onChange)="onRegionChange($event)"
          [appendTo]="'body'"
        >
          <ng-template let-region pTemplate="item">
            <span>{{ region.name }}</span>
          </ng-template>
        </p-dropdown>
      </div>
      <div class="responsive-dropdown col">
        <p-dropdown
          [options]="cities"
          formControlName="cityId"
          optionLabel="name"
          optionValue="id"
          [placeholder]="'address.city.placeholder' | translate"
          [disabled]="!form.get('regionId')?.value"
          (onChange)="onCityChange($event)"
          [appendTo]="'body'"
        >
          <ng-template let-city pTemplate="item">
            <span>{{ city.name }}</span>
          </ng-template>
        </p-dropdown>
      </div>
</div>
  `,
  styles: [`
    :host {
      display: block;
    }

    :host ::ng-deep .p-dropdown {
      width: 100%;
    }

    :host ::ng-deep .p-dropdown-panel {
      width: auto !important;
      min-width: 100%;
    }
  `]
})
export class AddressSelectComponent implements OnInit {
  @Input() initialRegionId?: number;
  @Input() initialCityId?: number;
  @Input() initialRegionName?: string;
  @Input() initialCityName?: string;
  @Input() form!: FormGroup;
  @Output() addressChange = new EventEmitter<{ cityId: number, regionId?: number }>();

  regions: Region[] = [];
  cities: City[] = [];

  constructor(
    private readonly addressService: AddressService
  ) {
  }

  ngOnInit(): void {
    this.loadRegions();
    if (this.initialRegionId && this.initialCityId) {
      this.form.patchValue({
        regionId: this.initialRegionId,
        cityId: this.initialCityId
      });
      this.loadCities(this.initialRegionId);
    }
  }

  private loadRegions(): void {
    this.addressService.getRegions().subscribe((response: BaseApiResponse<Region[]>) => {
      this.regions = response.value ?? [];
    });
  }

  private loadCities(regionId: number): void {
    this.addressService.getCitiesByRegion(regionId).subscribe((response: BaseApiResponse<City[]>) => {
      this.cities = response.value ?? [];
    });
  }

  onRegionChange(event: any): void {
    const regionId = event.value;
    this.form.patchValue({ cityId: null });
    this.cities = [];
    
    if (regionId) {
      this.loadCities(regionId);
    }
    
    this.emitAddressChange();
  }

  onCityChange(event: any): void {
    this.emitAddressChange();
  }

  private emitAddressChange(): void {
    const { cityId, regionId } = this.form.value;
    this.addressChange.emit({ cityId, regionId });
  }
}
