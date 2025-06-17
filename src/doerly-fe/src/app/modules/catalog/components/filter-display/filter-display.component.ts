import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccordionModule } from 'primeng/accordion';
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
    InputNumberModule,
    SliderModule,
    TranslateModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class FilterDisplayComponent implements OnChanges {
  @Input() filters: IFilter[] = [];
  @Output() filterChange = new EventEmitter<any[]>();

  private selectedFilterValues: Map<number, Set<string>> = new Map();

  ngOnChanges(changes: SimpleChanges) {
    if (changes['filters'] && changes['filters'].currentValue) {
      this.initializeFilterState();
    }
  }

  private initializeFilterState(): void {
    this.selectedFilterValues.clear();
    this.filters.forEach(filter => {
      this.selectedFilterValues.set(filter.id, new Set());
      
      if (filter.type === 1 || filter.type === 2) {
        filter.values.forEach(value => {
          value.selected = false;
        });
      } else if (filter.type === 3) {
        const min = filter.min ?? 0;
        const max = filter.max ?? 100;
        filter.minValue = min;
        filter.maxValue = max;
        filter.range = [min, max];
      } else if (filter.type === 4) {
        filter.selectedValue = undefined;
      }
    });
  }

  onFilterChange(): void {
    const filterValues = this.getFilterValues();
    this.filterChange.emit(filterValues);
  }

  onCheckboxChange(filter: IFilter, value: FilterValue): void {
    value.selected = !value.selected;
    const selectedValues = this.selectedFilterValues.get(filter.id) || new Set();
    
    if (value.selected) {
      selectedValues.add(value.id);
    } else {
      selectedValues.delete(value.id);
    }
    
    this.selectedFilterValues.set(filter.id, selectedValues);
    this.onFilterChange();
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
          const selectedValues = this.selectedFilterValues.get(filter.id);
          if (selectedValues) {
            selectedValues.forEach(valueId => {
              filterValues.push({
                filterId: filter.id,
                value: valueId
              });
            });
          }
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