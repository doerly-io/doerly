import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccordionModule } from 'primeng/accordion';
import { CheckboxModule } from 'primeng/checkbox';
import { InputNumberModule } from 'primeng/inputnumber';
import { SliderModule } from 'primeng/slider';
import { TranslateModule } from '@ngx-translate/core';
import { IFilter } from '../../models/filter.model';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

interface FilterValue {
  id: string;
  name: string;
  selected?: boolean;
}

interface FilterRange {
  min: number;
  max: number;
  minValue?: number;
  maxValue?: number;
  range?: [number, number];
}

@Component({
  selector: 'app-filter-display',
  templateUrl: './filter-display.component.html',
  styleUrls: ['./filter-display.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AccordionModule,
    CheckboxModule,
    InputNumberModule,
    SliderModule,
    TranslateModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class FilterDisplayComponent {
  @Input() filters: IFilter[] = [];
  @Output() filterChange = new EventEmitter<any[]>();

  onFilterChange(): void {
    const filterValues = this.getFilterValues();
    this.filterChange.emit(filterValues);
  }

  onRangeChange(event: any, filter: IFilter): void {
    if (filter.type === 3) {
      filter.minValue = event.values[0];
      filter.maxValue = event.values[1];
      this.onFilterChange();
    }
  }

  private getFilterValues(): any[] {
    const filterValues: any[] = [];

    this.filters.forEach(filter => {
      switch (filter.type) {
        case 1: // Checkbox
        case 2: // Dropdown (now treated as checkbox)
          filter.values.forEach((value: FilterValue) => {
            if (value.selected) {
              filterValues.push({
                filterId: filter.id,
                value: value.id
              });
            }
          });
          break;

        case 3: // Numeric Range
          const rangeFilter = filter as IFilter & FilterRange;
          if (rangeFilter.minValue !== undefined && rangeFilter.maxValue !== undefined) {
            filterValues.push({
              filterId: filter.id,
              value: rangeFilter.minValue.toString()
            });
            filterValues.push({
              filterId: filter.id,
              value: rangeFilter.maxValue.toString()
            });
          }
          break;

        case 4: // Radio (now treated as boolean)
          if (filter.selectedValue) {
            filterValues.push({
              filterId: filter.id,
              value: 'true'
            });
          }
          break;
      }
    });

    return filterValues;
  }

  getMinValue(filter: IFilter): number {
    return Math.min(...filter.values.map(v => Number(v)));
  }

  getMaxValue(filter: IFilter): number {
    return Math.max(...filter.values.map(v => Number(v)));
  }
} 