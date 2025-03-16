import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { BaseApiResponse } from "../../../@core/models/base-api-response";
import { Observable } from "rxjs";
import { GetOrdersResponse } from "../models/responses/get-orders-response";
import { setPaginationParams } from "../../../@core/helpers/pagination.helper";
import { GetExecutionProposalsWithPaginationByPredicatesRequest } from "../models/requests/get-execution-proposals-request";
import { GetExecutionProposalsResponse } from "../models/responses/get-execution-proposals-response";

@Injectable({
  providedIn: 'root'
})
export class ExecutionProposalService {
  private baseUrl = environment.baseApiUrl + '/execution-proposal'

  constructor(private readonly httpClient: HttpClient) {}

  getExecutionProposalsWithPagination(model: GetExecutionProposalsWithPaginationByPredicatesRequest): Observable<BaseApiResponse<GetExecutionProposalsResponse>> {
    // let params = setPaginationParams(model.pageInfo);
    // if (model.receiverId != null)
    //     params = params.set('ReceiverId', model.receiverId.toString());
    // if (model.senderId != null)
    //     params = params.set('SenderId', model.senderId.toString());

    return this.httpClient.post<BaseApiResponse<GetExecutionProposalsResponse>>(`${this.baseUrl}/list`, model);
  }
}