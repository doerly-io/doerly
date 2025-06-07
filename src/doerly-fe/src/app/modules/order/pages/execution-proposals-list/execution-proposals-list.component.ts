import { Component, Input, OnInit } from '@angular/core';
import { ExecutionProposalService } from '../../domain/execution-proposal.service';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { DataView } from 'primeng/dataview';
import { Tag } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { GetExecutionProposalResponse } from '../../models/responses/get-execution-proposal-response';
import { PaginatorModule } from 'primeng/paginator';
import { EExecutionProposalStatus } from '../../domain/enums/execution-proposal-status';
import { GetExecutionProposalsWithPaginationByPredicatesRequest } from '../../models/requests/get-execution-proposals-request';
import { TranslatePipe } from '@ngx-translate/core';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { Avatar } from 'primeng/avatar';
import { getExecutionProposalStatusSeverity } from '../../domain/enums/execution-proposal-status';
import { ErrorHandlerService } from '../../domain/error-handler.service';

@Component({
  selector: 'app-execution-proposals-list',
  imports: [
    DataView,
    Tag,
    PaginatorModule,
    CommonModule,
    TranslatePipe,
    RouterLink,
    Avatar
  ],
  templateUrl: './execution-proposals-list.component.html',
  styleUrl: './execution-proposals-list.component.scss'
})
export class ExecutionProposalsListComponent implements OnInit {

  @Input() senderId?: number | null;
  @Input() receiverId?: number | null;

  proposals: GetExecutionProposalResponse[] = [];
  totalRecords: number = 0;
  loading: boolean = true;
  returnUrl!: string;
  EExecutionProposalStatus = EExecutionProposalStatus;
  public getExecutionProposalStatusSeverity = getExecutionProposalStatusSeverity;

  constructor(private executionProposalService: ExecutionProposalService,
    private route: ActivatedRoute,
    private errorHandler: ErrorHandlerService) { }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['return'];
  }

  loadProposals(event: any) {
    this.loading = true;
    const request: GetExecutionProposalsWithPaginationByPredicatesRequest = {
      pageInfo: {
        number: event.first / event.rows + 1,
        size: event.rows
      },
      senderId: this.senderId,
      receiverId: this.receiverId,
    };
    this.executionProposalService.getExecutionProposalsWithPagination(request).subscribe({
      next: (response) => {
        this.proposals = response.value?.executionProposals || [];
        this.totalRecords = response.value?.total || 0;
        this.loading = false;
      },
      error: (error) => this.errorHandler.handleApiError(error)
    });
  }
}