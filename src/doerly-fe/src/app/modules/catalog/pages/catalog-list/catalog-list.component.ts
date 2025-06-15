import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { CatalogService } from '../../domain/catalog.service';
import { IService } from '../../models/service.model';
import { DropdownModule } from 'primeng/dropdown';
import { PaginatorModule } from 'primeng/paginator';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import { FilterDisplayComponent } from '../../components/filter-display/filter-display.component';
import { IFilter } from '../../models/filter.model';
import { SearchService } from 'app/@core/services/search.service';
import { Subscription } from 'rxjs';

interface SortOption {
  label: string;
  value: string;
}

@Component({
  selector: 'app-catalog-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    DropdownModule,
    PaginatorModule,
    TranslateModule,
    FormsModule,
    CardModule,
    ButtonModule,
    FilterDisplayComponent
  ],
  templateUrl: './catalog-list.component.html',
  styleUrls: ['./catalog-list.component.scss']
})
export class CatalogListComponent implements OnInit, OnDestroy {
  services: IService[] = [];
  filters: IFilter[] = [];
  isAuthorized: boolean = false;
  isAuthPage: boolean = false;
  isLoadingFilters: boolean = false;
  selectedFilters: any[] = [];
  sortOptions: SortOption[] = [
    { label: 'catalog.sort_options.price_asc', value: 'price_asc' },
    { label: 'catalog.sort_options.price_desc', value: 'price_desc' },
    { label: 'catalog.sort_options.name_asc', value: 'name_asc' },
    { label: 'catalog.sort_options.name_desc', value: 'name_desc' }
  ];
  selectedSort: string = 'price_asc';
  private searchSubscription: Subscription;
  
  pagination = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0
  };

  categoryId: number | null = null;

  constructor(
    private catalogService: CatalogService, 
    private route: ActivatedRoute,
    private router: Router,
    private jwtTokenHelper: JwtTokenHelper,
    private searchService: SearchService
  ) {
    this.searchSubscription = this.searchService.searchValue$.subscribe(searchValue => {
      if (searchValue) {
        this.categoryId = null;
        this.pagination.pageNumber = 1;
        this.loadServices();
      }
    });
  }

  ngOnInit() {
    this.isAuthorized = !!this.jwtTokenHelper.getUserInfo();
    this.route.url.subscribe(segments => {
      this.isAuthPage = segments.some(segment => segment.path === 'auth');
    });
    
    this.route.params.subscribe(params => {
      const newCategoryId = params['categoryId'] ? +params['categoryId'] : null;
      if (this.categoryId !== newCategoryId) {
        this.resetFilters();
      }
      this.categoryId = newCategoryId;
      this.pagination.pageNumber = 1;
      this.loadFilters();
      this.loadServices();
    });
  }

  ngOnDestroy() {
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
  }

  loadFilters(): void {
    if (!this.categoryId) return;
    
    this.isLoadingFilters = true;
    this.catalogService.getFiltersByCategoryId(this.categoryId).subscribe({
      next: (response) => {
        if (response && response.value) {
          this.filters = response.value.map((filter: IFilter) => ({
            ...filter,
            values: filter.values.map(value => ({
              id: value,
              name: value,
              selected: false
            }))
          }));
        } else {
          this.filters = [];
        }
        this.isLoadingFilters = false;
      },
      error: (error) => {
        console.error('Error loading filters:', error);
        this.filters = [];
        this.isLoadingFilters = false;
      }
    });
  }

  onFilterChange(filterValues: any[]): void {
    this.selectedFilters = filterValues;
    this.pagination.pageNumber = 1;
    this.loadServices();
  }

  loadServices() {
    const requestBody = {
      pageInfo: {
        number: this.pagination.pageNumber,
        size: this.pagination.pageSize
      },
      categoryId: this.categoryId || undefined,
      filterValues: this.selectedFilters,
      sortBy: this.selectedSort,
      searchValue: this.searchService.getSearchValue()
    };

    this.catalogService.getServicesWithPagination(requestBody).subscribe({
      next: (response: any) => {
        if (response.isSuccess && response.value) {
          this.services = response.value.services;
          this.pagination.totalCount = response.value.total;
        }
      },
      error: (error: any) => {
        console.error('Error loading services:', error);
      }
    });
  }

  onSortChange(event: any) {
    this.selectedSort = event.value;
    this.loadServices();
  }

  onPageChange(event: any) {
    this.pagination.pageNumber = event.page + 1;
    this.pagination.pageSize = event.rows;
    this.loadServices();
  }

  private resetFilters(): void {
    this.selectedFilters = [];
    this.filters = [];
    this.selectedSort = 'price_asc';
  }
} 