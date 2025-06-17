import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ExecutionProposalService } from '../../domain/execution-proposal.service';
import { GetExecutionProposalResponse } from '../../models/responses/get-execution-proposal-response';
import { EExecutionProposalStatus, getExecutionProposalStatusSeverity } from '../../domain/enums/execution-proposal-status';
import { ResolveExecutionProposalRequest } from '../../models/requests/resolve-execution-proposal-request';
import { CommonModule } from '@angular/common';
import { Card } from 'primeng/card';
import { Button } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { HttpErrorResponse } from '@angular/common/http';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import { Avatar } from 'primeng/avatar';
import { Tooltip } from 'primeng/tooltip';
import { Tag } from 'primeng/tag';
import { PanelModule } from 'primeng/panel';
import { ErrorHandlerService } from '../../../../@core/services/error-handler.service';

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
    RouterLink,
    Avatar,
    Tag,
    PanelModule
  ]
})
export class ExecutionProposalDetailsComponent implements OnInit {
  proposal: GetExecutionProposalResponse | null = null;
  loading: boolean = true;
  profileId: number;
  EExecutionProposalStatus = EExecutionProposalStatus;
  public getExecutionProposalStatusSeverity = getExecutionProposalStatusSeverity;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private executionProposalService: ExecutionProposalService,
    private toastHelper: ToastHelper,
    private readonly jwtTokenHelper: JwtTokenHelper,
    private errorHandler: ErrorHandlerService
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
        error: (error) => this.errorHandler.handleApiError(error)
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
        this.router.navigate(['/ordering'], { queryParams: { tab: 1, subTab: 0 } });
      },
      error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
    });
  }

  get profileAvatarUrl(): string | null | undefined {
    if (!this.proposal) return null;
    // Якщо я відправник — показуємо отримувача, якщо я отримувач — показуємо відправника
    if (this.proposal.senderId === this.profileId) {
      return this.proposal.receiver?.avatarUrl;
    } else {
      return this.proposal.sender?.avatarUrl;
    }
  }

  get profileName(): string {
    if (!this.proposal) return '';
    if (this.proposal.senderId === this.profileId) {
      return `${this.proposal.receiver?.firstName || ''} ${this.proposal.receiver?.lastName || ''}`.trim();
    } else {
      return `${this.proposal.sender?.firstName || ''} ${this.proposal.sender?.lastName || ''}`.trim();
    }
  }
}