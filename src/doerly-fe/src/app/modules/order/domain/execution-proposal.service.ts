import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { BaseApiResponse } from "../../../@core/models/base-api-response";
import { Observable } from "rxjs";  
import { GetExecutionProposalsWithPaginationByPredicatesRequest } from "../models/requests/get-execution-proposals-request";
import { GetExecutionProposalsResponse } from "../models/responses/get-execution-proposals-response";
import { GetExecutionProposalResponse } from "../models/responses/get-execution-proposal-response";
import { ResolveExecutionProposalRequest } from "../models/requests/resolve-execution-proposal-request";
import { SendExecutionProposalRequest } from "../models/requests/send-execution-proposal-request";

@Injectable({
  providedIn: 'root'
})
export class ExecutionProposalService {
  private baseUrl = environment.baseApiUrl + '/order/executionproposal';

  constructor(private readonly httpClient: HttpClient) {}

  getExecutionProposalsWithPagination(model: GetExecutionProposalsWithPaginationByPredicatesRequest): Observable<BaseApiResponse<GetExecutionProposalsResponse>> {
    return this.httpClient.post<BaseApiResponse<GetExecutionProposalsResponse>>(`${this.baseUrl}/list`, model);
  }

  sendExecutionProposal(model: SendExecutionProposalRequest): Observable<BaseApiResponse<number>> {
    return this.httpClient.post<BaseApiResponse<number>>(`${this.baseUrl}`, model); 
  }

  resolveExecutionProposal(model: ResolveExecutionProposalRequest): Observable<BaseApiResponse<number>> {
    return this.httpClient.post<BaseApiResponse<number>>(`${this.baseUrl}/resolve`, model);
  }

  getExecutionProposal(id: number): Observable<BaseApiResponse<GetExecutionProposalResponse>> {
    return this.httpClient.get<BaseApiResponse<GetExecutionProposalResponse>>(`${this.baseUrl}/${id}`);
  }
}