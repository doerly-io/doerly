import { Component, OnInit } from '@angular/core';
import { ExecutionProposalService } from '../../domain/execution-proposal.service';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { DataView } from 'primeng/dataview';
import { Tag } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { GetExecutionProposalResponse } from '../../models/responses/get-execution-proposal-response';
import { PaginatorModule } from 'primeng/paginator';
import { ExecutionProposalStatus } from '../../domain/enums/execution-proposal-status';
import { GetExecutionProposalsRequest } from '../../models/requests/get-execution-proposals-request';

@Component({
  selector: 'app-execution-proposals-list',
  imports: [
    DataView,
    Tag,
    PaginatorModule,
    CommonModule
  ],
  templateUrl: './execution-proposals-list.component.html',
  styleUrl: './execution-proposals-list.component.scss'
})
export class ExecutionProposalsListComponent implements OnInit {

  proposals: GetExecutionProposalResponse[] = [];
  totalRecords: number = 0;
  loading: boolean = true;
  returnUrl!: string;

  constructor(private executionProposalService: ExecutionProposalService,
                private router: Router,
                private route: ActivatedRoute) {}

  ngOnInit() { 
    this.returnUrl = this.route.snapshot.queryParams['return'];
  }

  loadProposals(event: any) {
    this.loading = true;
    const request: GetExecutionProposalsRequest = {
      pageInfo: {
        number: event.first / event.rows + 1,
        size: event.rows
      },
      receiverId: null,
      senderId: null
    };
    this.executionProposalService.getExecutionProposalsWithPagination(request).subscribe({
      next: (response) => {
        this.proposals = response.value?.executionProposals || [];
        this.totalRecords = response.value?.total || 0;
        this.loading = false;
        console.log(this.proposals);
      },
      error: (error) => {
        console.log(error);
        this.loading = false;
      }
    });
  }

  getProposalStatusString(status: ExecutionProposalStatus): string {
      return ExecutionProposalStatus[status];
  }

  navigateToProposalDetails(proposalId: number): void {
    this.router.navigate(['/execution-proposal', proposalId]);
  }
}