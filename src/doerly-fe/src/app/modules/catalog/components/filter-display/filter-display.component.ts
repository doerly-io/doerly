import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { DropdownModule } from 'primeng/dropdown';
import { SliderModule } from 'primeng/slider';
import { TranslateModule } from '@ngx-translate/core';
import { IFilter } from '../../models/filter.model';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

@Component({
  selector: 'app-filter-display',
  templateUrl: './filter-display.component.html',
  styleUrls: ['./filter-display.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CheckboxModule,
    RadioButtonModule,
    DropdownModule,
    SliderModule,
    TranslateModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class FilterDisplayComponent {
  @Input() filters: IFilter[] = [];

  getMinValue(filter: IFilter): number {
    return Math.min(...filter.values.map(v => Number(v)));
  }

  getMaxValue(filter: IFilter): number {
    return Math.max(...filter.values.map(v => Number(v)));
  }
} 