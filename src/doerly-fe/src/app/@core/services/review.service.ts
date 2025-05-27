import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {UpsertReview} from 'app/@core/models/review/add-review.model';
import {Observable} from 'rxjs';
import {environment} from 'environments/environment.development';
import {CursorPaginationRequest} from "app/@core/models/cursor-pagination-request.model";
import {ReviewResponse} from "app/@core/models/review/review-reponse.model";

@Injectable({
    providedIn: 'root'
})
export class ReviewService {

    private readonly baseUrl = environment.baseApiUrl + '/profile';
    private readonly httpClient: HttpClient = inject(HttpClient);

    addReview(profileId: number, review: UpsertReview): Observable<any> {
        const url = `${this.baseUrl}/${profileId}/reviews`;
        return this.httpClient.post(url, review);
    }

    getReviews(profileId: number, cursor: CursorPaginationRequest): Observable<ReviewResponse[]> {
        const url = `${this.baseUrl}/${profileId}/reviews`;
        const params = {
            cursor: cursor.cursor ?? '',
            pageSize: cursor.pageSize.toString()
        };
        return this.httpClient.get<ReviewResponse[]>(url, {params: params as Record<string, string>});
    }

    updateReview(profileId: number, reviewId: number, review: UpsertReview): Observable<any> {
        const url = `${this.baseUrl}/${profileId}/reviews/${reviewId}`;
        return this.httpClient.put(url, review);
    }

    deleteReview(profileId: number, reviewId: number): Observable<any> {
        const url = `${this.baseUrl}/${profileId}/reviews/${reviewId}`;
        return this.httpClient.delete(url);
    }


}
