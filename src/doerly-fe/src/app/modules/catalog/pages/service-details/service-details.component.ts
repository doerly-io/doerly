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
  template: `
    <div class="service-details-container" *ngIf="service">
      <div class="service-header">
        <div class="header-content">
          <div class="title-section">
            <h1>{{service.name}}</h1>
            <span class="category">{{service.categoryName}}</span>
          </div>
          <div class="price-tag">
            {{service.price | currency:'UAH':'symbol-narrow':'1.0-0'}}
          </div>
        </div>
        <div class="create-order-button">
          <p-button 
            icon="pi pi-shopping-cart" 
            [routerLink]="'/ordering/create-order'"
            [queryParams]="{
              categoryId: service.categoryId,
              executorId: service.userId,
              name: service.name,
              description: service.description,
              price: service.price
            }"
            label="{{ 'ordering.create_order' | translate }}"
            [severity]="'success'"
            styleClass="w-100">
          </p-button>
        </div>
      </div>

      <div class="service-content">
        <div class="main-info">
          <p-card>
            <div class="description">
              <h2>{{ 'catalog.description' | translate }}</h2>
              <p class="description-text">{{service.description}}</p>
            </div>

            <div class="filters" *ngIf="service.filterValues?.length">
              <h2>{{ 'catalog.additionalInfo' | translate }}</h2>
              <div class="filter-grid">
                <ng-container *ngFor="let filterGroup of groupedFilters">
                  <div class="filter-item">
                    <span class="filter-name">{{filterGroup.name}}:</span>
                    <div class="filter-values">
                      <ng-container *ngIf="filterGroup.type === 1 || filterGroup.type === 2">
                        <span class="filter-value" *ngFor="let value of filterGroup.values; let last = last">
                          {{value}}{{!last ? ', ' : ''}}
                        </span>
                      </ng-container>
                      <ng-container *ngIf="filterGroup.type === 3">
                        <span class="filter-value">{{filterGroup.values[0]}}</span>
                      </ng-container>
                      <ng-container *ngIf="filterGroup.type === 4">
                        <span class="filter-value" [ngClass]="{'boolean-value': true}">
                          {{filterGroup.values[0] === 'true' ? ('common.yes' | translate) : ('common.no' | translate)}}
                        </span>
                      </ng-container>
                    </div>
                  </div>
                </ng-container>
              </div>
            </div>
          </p-card>
        </div>

        <div class="provider-info">
          <p-card>
            <div class="provider-header">
              <h2>{{ 'catalog.provider' | translate }}</h2>
            </div>

            <div class="provider-card">
              <img [src]="service.user.imageUrl || 'assets/images/default-avatar.png'" 
                   [alt]="service.user.firstName + ' ' + service.user.lastName"
                   class="provider-avatar">
              <div class="provider-details">
                <h3>{{service.user.firstName}} {{service.user.lastName}}</h3>
                <p class="location" *ngIf="service.user.address">
                  <i class="pi pi-map-marker"></i>
                  {{service.user.address.cityName}}, {{service.user.address.regionName}}
                </p>
                <p class="bio" *ngIf="service.user.bio">
                  <i class="pi pi-user"></i>
                  {{service.user.bio}}
                </p>
              </div>
            </div>

            <div class="provider-competences" *ngIf="service.user.competences?.length">
              <h3>{{ 'catalog.competences' | translate }}</h3>
              <div class="competence-tags">
                <p-tag *ngFor="let competence of service.user.competences" 
                       [value]="competence.categoryName"
                       severity="info"
                       styleClass="competence-tag">
                </p-tag>
              </div>
            </div>
          </p-card>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .service-details-container {
      max-width: 1200px;
      margin: 2rem auto;
      padding: 0 1rem;
    }

    .service-header {
      background-color: var(--surface-card);
      border-radius: 8px;
      padding: 2rem;
      margin-bottom: 2rem;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);

      .header-content {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 2rem;
        color: var(--text-color);
        margin-bottom: 1.5rem;

        @media (max-width: 768px) {
          flex-direction: column;
          align-items: flex-start;
          gap: 1rem;
        }
      }

      .title-section {
        h1 {
          margin: 0;
          color: var(--text-color);
          font-size: 2rem;
          font-weight: 600;
          word-break: break-word;
        }

        .category {
          color: var(--text-color-secondary);
          font-size: 1.1rem;
          display: block;
          margin-top: 0.5rem;
        }
      }

      .price-tag {
        font-size: 2rem;
        font-weight: 600;
        color: var(--primary-color);
        white-space: nowrap;
      }

      .create-order-button {
        :host ::ng-deep .p-button {
          width: 100%;
          padding: 1rem;
          font-size: 1.1rem;
        }
      }
    }

    .service-content {
      display: grid;
      grid-template-columns: 2fr 1fr;
      gap: 2rem;

      @media (max-width: 1024px) {
        grid-template-columns: 1fr;
      }
    }

    .main-info, .provider-info {
      :host ::ng-deep .p-card {
        height: 100%;
        background-color: var(--surface-card);
        
        .p-card-body {
          height: 100%;
        }
      }
    }

    .description, .filters {
      margin-bottom: 2rem;

      h2 {
        color: var(--text-color);
        font-size: 1.5rem;
        margin-bottom: 1rem;
        font-weight: 600;
      }
    }

    .description-text {
      color: var(--text-color);
      line-height: 1.6;
      margin: 0;
      word-break: break-word;
      white-space: pre-wrap;
    }

    .filter-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
      gap: 1rem;
    }

    .filter-item {
      background-color: var(--surface-hover);
      padding: 1rem;
      border-radius: 8px;

      .filter-name {
        font-weight: 600;
        color: var(--text-color);
        display: block;
        margin-bottom: 0.5rem;
      }

      .filter-values {
        color: var(--text-color-secondary);
        word-break: break-word;
        white-space: pre-wrap;
      }

      .filter-value {
        &.boolean-value {
          font-weight: 500;
        }
      }
    }

    .provider-header {
      margin-bottom: 1.5rem;

      h2 {
        color: var(--text-color);
        font-size: 1.5rem;
        margin: 0;
        font-weight: 600;
      }
    }

    .provider-card {
      display: flex;
      gap: 1.5rem;
      margin-bottom: 2rem;
      padding-bottom: 1.5rem;
      border-bottom: 1px solid var(--surface-border);

      @media (max-width: 480px) {
        flex-direction: column;
        align-items: center;
        text-align: center;
      }
    }

    .provider-avatar {
      width: 100px;
      height: 100px;
      border-radius: 50%;
      object-fit: cover;
      border: 3px solid var(--surface-border);
    }

    .provider-details {
      h3 {
        margin: 0 0 0.5rem 0;
        color: var(--text-color);
        font-size: 1.2rem;
        word-break: break-word;
      }

      .location, .bio {
        color: var(--text-color-secondary);
        margin: 0.5rem 0;
        display: flex;
        align-items: center;
        gap: 0.5rem;
        word-break: break-word;

        @media (max-width: 480px) {
          justify-content: center;
        }

        i {
          font-size: 1rem;
        }
      }
    }

    .provider-competences {
      h3 {
        color: var(--text-color);
        font-size: 1.2rem;
        margin: 0 0 1rem 0;
      }
    }

    .competence-tags {
      display: flex;
      flex-wrap: wrap;
      gap: 0.5rem;

      :host ::ng-deep .p-tag {
        font-size: 0.9rem;
        padding: 0.5rem 1rem;
        word-break: break-word;
      }
    }
  `]
})
export class ServiceDetailsComponent implements OnInit {
  service: IServiceDetails | null = null;
  groupedFilters: { name: string; type: number; values: string[] }[] = [];

  constructor(
    private catalogService: CatalogService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const serviceId = params['id'];
      if (serviceId) {
        this.loadServiceDetails(serviceId);
      }
    });
  }

  loadServiceDetails(serviceId: number) {
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