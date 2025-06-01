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

@Component({
  selector: 'app-execution-proposals-list',
  imports: [
    DataView,
    Tag,
    PaginatorModule,
    CommonModule,
    TranslatePipe,
    RouterLink
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

  constructor(private executionProposalService: ExecutionProposalService,
                private toastHelper: ToastHelper,
                private route: ActivatedRoute) {}

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
      error: (error) => {
        if (error.status === 400) {
          this.toastHelper.showError('common.error', error.error.errorMessage);
        }
        else {
          this.toastHelper.showError('common.error', 'common.error-occurred');
        }
      }
    });
  }

  getProposalStatusSeverity(status: EExecutionProposalStatus): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined {
    switch (status) {
      case EExecutionProposalStatus.Pending:
        return 'info';
      case EExecutionProposalStatus.Accepted:
        return 'success';
      case EExecutionProposalStatus.Rejected:
        return 'danger';
      case EExecutionProposalStatus.Revoked:
        return 'warn';
      default:
        return 'info';
    }
  }
}