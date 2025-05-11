import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExecutionProposalService } from '../../domain/execution-proposal.service';
import { GetExecutionProposalResponse } from '../../models/responses/get-execution-proposal-response';
import { EExecutionProposalStatus } from '../../domain/enums/execution-proposal-status';
import { ResolveExecutionProposalRequest } from '../../models/requests/resolve-execution-proposal-request';
import { CommonModule } from '@angular/common';
import { Card } from 'primeng/card';
import { Button } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-execution-proposal-details',
  templateUrl: './execution-proposal-details.component.html',
  styleUrls: ['./execution-proposal-details.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    Card,
    Button,
    TranslatePipe
  ]
})
export class ExecutionProposalDetailsComponent implements OnInit {
  proposal: GetExecutionProposalResponse | null = null;
  loading: boolean = true;
  profileId: number = 2; //for testing purposes

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private executionProposalService: ExecutionProposalService,
    private toastHelper: ToastHelper
  ) {}

  ngOnInit(): void {
    const proposalId = Number(this.route.snapshot.paramMap.get('id'));
    if (proposalId) {
      this.loading = true;
      this.executionProposalService.getExecutionProposal(proposalId).subscribe({
        next: (response) => {
          this.proposal = response.value || null;
          this.loading = false;
        },
        error: (error) => {
          console.error(error);
          this.loading = false;
        }
      });
    }
  }

  resolveProposal(status: EExecutionProposalStatus): void {
    console.log('resolveProposal', status);
    if (!this.proposal) return;

    const request: ResolveExecutionProposalRequest = {
      id: this.proposal.id,
      status: status
    };

    console.log('resolveProposal', request);

    this.executionProposalService.resolveExecutionProposal(request).subscribe({
      next: () => {
        this.toastHelper.showSuccess('common.success', 'ordering.resolved-successfully');
        this.router.navigate(['/ordering'], { queryParams: { tab: 0, subTab: 0 } });
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 400) {
          this.toastHelper.showError('common.error', error.error.errorMessage);
        }
        else {
          this.toastHelper.showError('common.error', 'common.error-occurred');
        }
      }
    });
  }

  getProposalStatusString(status: EExecutionProposalStatus): string {
    return EExecutionProposalStatus[status];
  }
}