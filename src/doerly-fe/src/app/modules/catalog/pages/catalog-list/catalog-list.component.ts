import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { CatalogService } from '../../domain/catalog.service';
import { IService } from '../../models/service.model';
import { DropdownModule } from 'primeng/dropdown';
import { PaginatorModule } from 'primeng/paginator';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';

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
    FormsModule
  ],
  templateUrl: './catalog-list.component.html',
  styleUrls: ['./catalog-list.component.scss']
})
export class CatalogListComponent implements OnInit {
  services: IService[] = [];
  sortOptions: SortOption[] = [
    { label: 'Name (A-Z)', value: 'name_asc' },
    { label: 'Name (Z-A)', value: 'name_desc' },
    { label: 'Price (Low to High)', value: 'price_asc' },
    { label: 'Price (High to Low)', value: 'price_desc' }
  ];
  selectedSort: string = 'name_desc';
  
  pagination = {
    pageNumber: 0,
    pageSize: 12,
    totalCount: 0
  };

  categoryId?: number;

  constructor(private catalogService: CatalogService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.categoryId = params['categoryId'] ? +params['categoryId'] : undefined;
      this.pagination.pageNumber = 0;
      this.loadServices();
    });
  }

  loadServices() {
    const requestBody = {
      pageNumber: this.pagination.pageNumber + 1,
      pageSize: this.pagination.pageSize,
      categoryId: this.categoryId,
      sortBy: this.selectedSort
    };
    console.log('Sending to service:', requestBody);
    this.catalogService.getServicesWithPagination({
      pageNumber: this.pagination.pageNumber + 1,
      pageSize: this.pagination.pageSize,
      categoryId: this.categoryId,
      sortBy: this.selectedSort
    }).subscribe({
      next: (response: any) => {
        console.log('Received from service:', response);
        if (response.isSuccess && response.value) {
          this.services = response.value.orders;
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
    this.pagination.pageNumber = event.page;
    this.pagination.pageSize = event.rows;
    this.loadServices();
  }
} 