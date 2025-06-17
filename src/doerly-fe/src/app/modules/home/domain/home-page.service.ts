import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "environments/environment.development";
import { GetOrdersAmountByCategoriesResponse } from "../models/get-orders-amount-by-categories-response";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class HomePageService {
    private baseUrl = environment.baseApiUrl + '/catalog/orders';
    private readonly http = inject(HttpClient);

    getOrdersAmountByCategories(): Observable<GetOrdersAmountByCategoriesResponse[]> {
        return this.http.get<GetOrdersAmountByCategoriesResponse[]>(`${this.baseUrl}/get-orders-amount-by-categories`);
    }
}
