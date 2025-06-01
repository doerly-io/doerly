import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
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
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';

@Component({
  selector: 'app-execution-proposal-details',
  templateUrl: './execution-proposal-details.component.html',
  styleUrls: ['./execution-proposal-details.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    Card,
    Button,
    TranslatePipe,
    RouterLink
  ]
})
export class ExecutionProposalDetailsComponent implements OnInit {
  proposal: GetExecutionProposalResponse | null = null;
  loading: boolean = true;
  profileId: number;
  EExecutionProposalStatus = EExecutionProposalStatus;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private executionProposalService: ExecutionProposalService,
    private toastHelper: ToastHelper,
    private readonly jwtTokenHelper: JwtTokenHelper
  ) {
    this.profileId = this.jwtTokenHelper.getUserInfo()?.id ?? 0;
  }

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
    if (!this.proposal) return;

    const request: ResolveExecutionProposalRequest = {
      status: status
    };

    console.log('resolveProposal', request);

    this.executionProposalService.resolveExecutionProposal(this.proposal.id, request).subscribe({
      next: () => {
        this.toastHelper.showSuccess('common.success', 'ordering.resolved_successfully');
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