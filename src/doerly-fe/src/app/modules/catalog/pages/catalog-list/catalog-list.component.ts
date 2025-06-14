import { Component, OnInit } from '@angular/core';
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
export class CatalogListComponent implements OnInit {
  services: IService[] = [];
  filters: IFilter[] = [];
  isAuthorized: boolean = false;
  isAuthPage: boolean = false;
  isLoadingFilters: boolean = false;
  sortOptions: SortOption[] = [
    { label: 'Price: Low to High', value: 'price_asc' },
    { label: 'Price: High to Low', value: 'price_desc' },
    { label: 'Name: A to Z', value: 'name_asc' },
    { label: 'Name: Z to A', value: 'name_desc' }
  ];
  selectedSort: string = 'price_asc';
  
  pagination = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0
  };

  categoryId: number = 2;

  constructor(
    private catalogService: CatalogService, 
    private route: ActivatedRoute,
    private router: Router,
    private jwtTokenHelper: JwtTokenHelper
  ) {}

  ngOnInit() {
    this.isAuthorized = !!this.jwtTokenHelper.getUserInfo();
    this.route.url.subscribe(segments => {
      this.isAuthPage = segments.some(segment => segment.path === 'auth');
    });
    
    this.route.params.subscribe(params => {
      this.categoryId = params['categoryId'] ? +params['categoryId'] : 2;
      this.pagination.pageNumber = 1;
      this.loadFilters();
      this.loadServices();
    });
  }

  loadFilters(): void {
    this.isLoadingFilters = true;
    console.log('Loading filters for category:', this.categoryId);
    this.catalogService.getFiltersByCategoryId(this.categoryId).subscribe({
      next: (response) => {
        console.log('Filters API Response:', response);
        if (response && response.value) {
          this.filters = response.value;
          console.log('Parsed filters:', this.filters);
        } else {
          console.warn('No filters data in response');
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

  loadServices() {
    const requestBody = {
      pageInfo: {
        number: this.pagination.pageNumber,
        size: this.pagination.pageSize
      },
      categoryId: this.categoryId,
      filterValues: [],
      sortBy: this.selectedSort
    };
    console.log('Sending to service:', requestBody);
    this.catalogService.getServicesWithPagination(requestBody).subscribe({
      next: (response: any) => {
        console.log('Received from service:', response);
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
} 