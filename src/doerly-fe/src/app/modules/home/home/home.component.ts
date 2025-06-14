import {Component, inject, OnInit, signal} from '@angular/core';
import {NgForOf, NgIf, NgOptimizedImage} from '@angular/common';
import {GetOrdersAmountByCategoriesResponse} from 'app/modules/home/models/get-orders-amount-by-categories-response';
import {HttpErrorResponse} from '@angular/common/http';
import {ErrorHandlerService} from 'app/@core/services/error-handler.service';
import {RouterLink} from '@angular/router';
import {Card} from 'primeng/card';
import {HomePageService} from 'app/modules/home/domain/home-page.service';

@Component({
  selector: 'app-home',
  imports: [
    NgOptimizedImage,
    RouterLink,
    NgForOf,
    NgIf,
    Card
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  private readonly homeService = inject(HomePageService);
  private readonly errorHandler = inject(ErrorHandlerService);

  protected categories = signal<GetOrdersAmountByCategoriesResponse[]>([]);

  ngOnInit(): void {
    this.loadPopularCategories();
  }

  loadPopularCategories(): void {
    this.homeService.getOrdersAmountByCategories().subscribe({
      next: (response) => {
        this.categories.set(response);
      },
      error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
    });


  }


}
