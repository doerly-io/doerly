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

      <div class="recommended-services" *ngIf="recommendedServices.length">
        <h2>{{ 'catalog.recommendedServices' | translate }}</h2>
        <div class="recommended-grid" [ngClass]="{
          'single-item': recommendedServices.length === 1,
          'two-items': recommendedServices.length === 2
        }">
          <p-card *ngFor="let service of recommendedServices" class="recommended-card">
            <div class="service-info">
              <div class="service-main">
                <div class="service-header">
                  <div class="title-section">
                    <h3 class="service-title">{{service.name}}</h3>
                    <span class="service-category">{{service.categoryName}}</span>
                  </div>
                </div>
                
                <p class="service-description">{{service.description}}</p>
                
                <div class="service-meta">
                  <div class="service-price">{{service.price | currency:'UAH':'symbol-narrow':'1.0-0'}}</div>
                </div>
              </div>

              <div class="service-sidebar">
                <div class="service-user" *ngIf="service.user">
                  <img [src]="service.user.imageUrl || 'assets/images/default-avatar.png'" 
                       [alt]="service.user.firstName + ' ' + service.user.lastName"
                       class="user-avatar">
                  <div class="user-info">
                    <span class="user-name">{{service.user.firstName}} {{service.user.lastName}}</span>
                    <span class="user-address" *ngIf="service.user.address">
                      {{service.user.address.cityName}}
                    </span>
                  </div>
                </div>

                <div class="service-actions">
                  <button pButton 
                          [routerLink]="['/catalog/service', service.id]"
                          label="{{ 'catalog.viewDetails' | translate }}"
                          class="p-button-primary">
                  </button>
                </div>
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

    .recommended-services {
      margin-top: 3rem;

      h2 {
        color: var(--text-color);
        font-size: 1.5rem;
        margin-bottom: 1.5rem;
        font-weight: 600;
      }
    }

    .recommended-grid {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      gap: 1.5rem;

      &.single-item {
        grid-template-columns: 1fr;
        max-width: 800px;
        margin: 0 auto;
      }

      &.two-items {
        grid-template-columns: repeat(2, 1fr);
        max-width: 1000px;
        margin: 0 auto;
      }

      @media (max-width: 1200px) {
        grid-template-columns: repeat(2, 1fr);
      }

      @media (max-width: 768px) {
        grid-template-columns: 1fr;
      }
    }

    .recommended-card {
      height: 100%;

      :host ::ng-deep .p-card {
        height: 100%;
        background-color: var(--surface-card);
        
        .p-card-body {
          height: 100%;
          padding: 1rem;
        }
      }
    }

    .service-info {
      display: flex;
      flex-direction: column;
      gap: 1rem;
      height: 100%;
    }

    .service-main {
      flex: 1;
    }

    .service-header {
      margin-bottom: 1rem;

      .title-section {
        .service-title {
          margin: 0;
          font-size: 1.25rem;
          font-weight: 600;
          color: var(--text-color);
          word-break: break-word;
        }

        .service-category {
          display: block;
          color: var(--text-color-secondary);
          font-size: 0.9rem;
          margin-top: 0.5rem;
        }
      }
    }

    .service-description {
      color: var(--text-color);
      margin: 0 0 1rem 0;
      line-height: 1.6;
      word-break: break-word;
      white-space: pre-wrap;
    }

    .service-meta {
      .service-price {
        font-size: 1.25rem;
        font-weight: 600;
        color: var(--primary-color);
      }
    }

    .service-sidebar {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .service-user {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      padding: 0.75rem;
      background-color: var(--surface-hover);
      border-radius: 8px;

      .user-avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        object-fit: cover;
      }

      .user-info {
        display: flex;
        flex-direction: column;
        gap: 0.25rem;

        .user-name {
          font-weight: 500;
          color: var(--text-color);
          word-break: break-word;
          font-size: 0.9rem;
        }

        .user-address {
          color: var(--text-color-secondary);
          font-size: 0.8rem;
          word-break: break-word;
        }
      }
    }

    .service-actions {
      :host ::ng-deep .p-button {
        width: 100%;
      }
    }
  `]
})
export class ServiceDetailsComponent implements OnInit {
  service: IServiceDetails | null = null;
  groupedFilters: { name: string; type: number; values: string[] }[] = [];

  recommendedServices: IService[] = [
    {
      id: 1,
      name: 'Репетитор з математики',
      description: 'Допомога з домашніми завданнями та підготовка до ЗНО з математики. Індивідуальний підхід до кожного учня.',
      categoryId: 12,
      categoryName: 'Освіта',
      userId: 1,
      user: {
        id: 1,
        firstName: 'Олександр',
        lastName: 'Петренко',
        sex: 1,
        dateCreated: '2024-03-20',
        lastModifiedDate: '2024-03-20',
        imageUrl: 'assets/images/default-avatar.png',
        address: {
          cityId: 1,
          cityName: 'Київ',
          regionId: 1,
          regionName: 'Київська область'
        },
        languageProficiencies: [],
        competences: []
      },
      price: 300,
      categoryPath: ['Освіта', 'Репетитори'],
      isEnabled: true,
      isDeleted: false,
      filterValues: [],
      createdDate: '2024-03-20'
    },
    {
      id: 2,
      name: 'Репетитор з фізики',
      description: 'Підготовка до ЗНО з фізики. Розбір складних тем та практичні завдання.',
      categoryId: 12,
      categoryName: 'Освіта',
      userId: 2,
      user: {
        id: 2,
        firstName: 'Марія',
        lastName: 'Коваленко',
        sex: 2,
        dateCreated: '2024-03-19',
        lastModifiedDate: '2024-03-19',
        imageUrl: 'assets/images/default-avatar.png',
        address: {
          cityId: 2,
          cityName: 'Львів',
          regionId: 2,
          regionName: 'Львівська область'
        },
        languageProficiencies: [],
        competences: []
      },
      price: 350,
      categoryPath: ['Освіта', 'Репетитори'],
      isEnabled: true,
      isDeleted: false,
      filterValues: [],
      createdDate: '2024-03-19'
    },
    {
      id: 3,
      name: 'Репетитор з англійської мови',
      description: 'Індивідуальні заняття з англійської мови для всіх рівнів. Розмовна практика та граматика.',
      categoryId: 12,
      categoryName: 'Освіта',
      userId: 3,
      user: {
        id: 3,
        firstName: 'Анна',
        lastName: 'Сидоренко',
        sex: 2,
        dateCreated: '2024-03-18',
        lastModifiedDate: '2024-03-18',
        imageUrl: 'assets/images/default-avatar.png',
        address: {
          cityId: 3,
          cityName: 'Харків',
          regionId: 3,
          regionName: 'Харківська область'
        },
        languageProficiencies: [],
        competences: []
      },
      price: 400,
      categoryPath: ['Освіта', 'Репетитори'],
      isEnabled: true,
      isDeleted: false,
      filterValues: [],
      createdDate: '2024-03-18'
    }
  ];

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
        this.loadRecommendedServices(+serviceId);
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

  private loadRecommendedServices(serviceId: number): void {
    // TODO: Replace with actual API call when available
    // For now, we're using mock data
    console.log('Loading recommended services for service:', serviceId);
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