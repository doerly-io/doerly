import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryService } from 'app/@core/services/category.service';
import { ICategory } from 'app/@core/models/category/category.model';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ButtonModule } from 'primeng/button';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-category-dropdown',
  standalone: true,
  imports: [
    CommonModule, 
    DropdownModule, 
    FormsModule, 
    TranslateModule, 
    ButtonModule, 
    OverlayPanelModule,
    RouterModule
  ],
  template: `
    <div class="category-dropdown">
      <p-button 
        [outlined]="true" 
        [text]="true"
        [label]="'Categories' | translate"
        icon="pi pi-bars"
        (click)="op.toggle($event)"
        styleClass="p-button-secondary category-button">
      </p-button>

      <p-overlayPanel #op [showCloseIcon]="true" [dismissable]="true" styleClass="category-panel">
        <div class="category-container">
          <div *ngFor="let category of categories" class="category-section">
            <a [routerLink]="['/catalog', category.id]" class="category-main" (click)="op.hide()">
              <h3>{{category.name}}</h3>
              <p class="category-description">{{category.description}}</p>
            </a>
            <div class="subcategories">
              <a *ngFor="let subCategory of category.children" 
                 [routerLink]="['/catalog', subCategory.id]"
                 class="subcategory-item"
                 (click)="op.hide()">
                {{subCategory.name}}
              </a>
            </div>
          </div>
        </div>
      </p-overlayPanel>
    </div>
  `,
  styles: [`
    .category-dropdown {
      position: relative;
    }

    .category-button {
      @media (max-width: 768px) {
        padding: 0.5rem;
        .p-button-label {
          display: none;
        }
      }
    }

    .category-panel {
      width: 95vw !important;
      max-width: 1400px !important;
      margin-top: 0.5rem;

      @media (max-width: 768px) {
        width: 100vw !important;
        max-width: 100vw !important;
        margin: 0;
        position: fixed !important;
        top: 0 !important;
        left: 0 !important;
        height: 100vh !important;
        border-radius: 0 !important;
      }
    }

    .category-container {
      display: flex;
      flex-wrap: wrap;
      gap: 2rem;
      padding: 1.5rem;

      @media (max-width: 768px) {
        flex-direction: column;
        gap: 1rem;
        padding: 1rem;
        overflow-y: auto;
        max-height: calc(100vh - 60px);
      }
    }

    .category-section {
      flex: 1;
      min-width: 300px;
      max-width: 400px;

      @media (max-width: 1200px) {
        min-width: 250px;
      }

      @media (max-width: 768px) {
        min-width: 100%;
        max-width: 100%;
      }
    }

    .category-main {
      display: block;
      text-decoration: none;
      color: var(--text-color);
      margin-bottom: 1rem;
      padding: 0.5rem;
      border-radius: 4px;
      transition: all 0.2s;

      &:hover {
        background-color: var(--surface-hover);
        color: var(--primary-color);
      }

      h3 {
        margin: 0;
        color: var(--primary-color);
        font-size: 1.2rem;
        font-weight: 600;

        @media (max-width: 768px) {
          font-size: 1.1rem;
        }
      }

      .category-description {
        margin: 0.5rem 0 0;
        color: var(--text-color-secondary);
        font-size: 0.9rem;

        @media (max-width: 768px) {
          font-size: 0.8rem;
        }
      }
    }

    .subcategories {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
      padding-left: 1rem;

      @media (max-width: 768px) {
        padding-left: 0.5rem;
      }
    }

    .subcategory-item {
      color: var(--text-color);
      text-decoration: none;
      padding: 0.5rem;
      border-radius: 4px;
      transition: all 0.2s;

      &:hover {
        background-color: var(--surface-hover);
        color: var(--primary-color);
      }

      @media (max-width: 768px) {
        padding: 0.75rem 0.5rem;
        font-size: 0.9rem;
      }
    }

    /* Mobile-specific styles */
    @media (max-width: 768px) {
      :host ::ng-deep {
        .p-overlaypanel-content {
          padding: 0;
        }
        
        .p-overlaypanel-close {
          top: 1rem;
          right: 1rem;
          background: var(--surface-card);
          border-radius: 50%;
          width: 2rem;
          height: 2rem;
          display: flex;
          align-items: center;
          justify-content: center;
          box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
      }
    }
  `]
})
export class CategoryDropdownComponent implements OnInit {
  categories: ICategory[] = [];

  constructor(private categoryService: CategoryService) {}

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe({
      next: (response) => {
        if (response.isSuccess && response.value) {
          this.categories = response.value;
        }
      },
      error: (error) => {
        console.error('Error loading categories:', error);
      }
    });
  }
} 