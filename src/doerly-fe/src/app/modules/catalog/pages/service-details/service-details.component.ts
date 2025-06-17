import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { CatalogService } from '../../domain/catalog.service';
import { TranslateModule } from '@ngx-translate/core';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { TagModule } from 'primeng/tag';
import { IServiceDetails } from '../../models/service-details.model';
import { IService } from '../../models/service.model';

@Component({
  selector: 'app-service-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
    CardModule,
    ButtonModule,
    DividerModule,
    TagModule
  ],
  templateUrl: './service-details.component.html',
  styleUrls: ['./service-details.component.scss']
})
export class ServiceDetailsComponent implements OnInit {
  service: IServiceDetails | null = null;
  groupedFilters: { name: string; type: number; values: string[] }[] = [];

  constructor(
    private route: ActivatedRoute,
    private catalogService: CatalogService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const serviceId = params['id'];
      if (serviceId) {
        this.loadServiceDetails(+serviceId);
      }
    });
  }

  private loadServiceDetails(serviceId: number): void {
    this.catalogService.getServiceDetails(serviceId).subscribe({
      next: (response) => {
        if (response.isSuccess && response.value) {
          this.service = response.value;
          this.groupFilters();
        }
      },
      error: (error) => {
        console.error('Error loading service details:', error);
      }
    });
  }

  private groupFilters() {
    if (!this.service?.filterValues) return;

    const filterMap = new Map<number, { name: string; type: number; values: string[] }>();

    this.service.filterValues.forEach(filter => {
      if (!filterMap.has(filter.filterId)) {
        filterMap.set(filter.filterId, {
          name: filter.filterName,
          type: filter.filterType,
          values: [filter.value]
        });
      } else {
        const existing = filterMap.get(filter.filterId)!;
        if (!existing.values.includes(filter.value)) {
          existing.values.push(filter.value);
        }
      }
    });

    this.groupedFilters = Array.from(filterMap.values());
  }
} 