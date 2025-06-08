import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExecutionProposalService } from '../../domain/execution-proposal.service';
import { GetExecutionProposalResponse } from '../../models/responses/get-execution-proposal-response';
import { UpdateExecutionProposalRequest } from '../../models/requests/update-execution-proposal-request';
import { SendExecutionProposalRequest } from '../../models/requests/send-execution-proposal-request';
import { HttpErrorResponse } from '@angular/common/http';
import { setServerErrors, getError, isInvalid, getServersideError } from 'app/@core/helpers/input-validation-helpers';
import { Card } from 'primeng/card';
import { ButtonDirective } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { NgIf } from '@angular/common';
import { ToastHelper } from 'app/@core/helpers/toast.helper';
import { JwtTokenHelper } from 'app/@core/helpers/jwtToken.helper';
import { TextAccessibilityManager } from 'pdfjs-dist/types/web/text_accessibility';
import { Textarea } from 'primeng/textarea';
import { ErrorHandlerService } from '../../../../@core/services/error-handler.service';

@Component({
  selector: 'app-edit-execution-proposal',
  templateUrl: './edit-execution-proposal.component.html',
  styleUrls: ['./edit-execution-proposal.component.scss'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    Card,
    ButtonDirective,
    TranslatePipe,
    NgIf,
    Textarea
  ]
})
export class EditExecutionProposalComponent implements OnInit {
  proposalForm!: FormGroup;
  proposalId?: number;
  orderId?: number;
  receiverId?: number;
  isEdit: boolean = false;
  loading: boolean = true;
  commentMaxLength: number = 1000;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private executionProposalService: ExecutionProposalService,
    private toastHelper: ToastHelper,
    private errorHandler: ErrorHandlerService
  ) { }

  ngOnInit() {
    this.proposalId = Number(this.route.snapshot.paramMap.get('id'));
    this.isEdit = !!this.proposalId;

    this.initForm();

    if (this.isEdit) {
      this.executionProposalService.getExecutionProposal(this.proposalId!).subscribe({
        next: (response) => {
          const proposal = response.value as GetExecutionProposalResponse;
          if (proposal) {
            this.proposalForm.patchValue({
              comment: proposal.comment
            });
          }
          this.loading = false;
        },
        error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
      });
    } else {
      this.loading = false;
      this.orderId = Number(this.route.snapshot.queryParamMap.get('orderId'));
      this.receiverId = Number(this.route.snapshot.queryParamMap.get('receiverId'));
    }
  }

  initForm(): void {
    this.proposalForm = this.formBuilder.group({
      comment: ['', [Validators.maxLength(this.commentMaxLength)]]
    });
  }

  submit(): void {
    if (this.proposalForm.invalid) return;

    if (this.isEdit) {
      const updatedProposal: UpdateExecutionProposalRequest = {
        comment: this.proposalForm.value.comment
      };
      this.executionProposalService.updateExecutionProposal(this.proposalId!, updatedProposal).subscribe({
        next: () => {
          this.toastHelper.showSuccess('common.success', 'ordering.execution_proposal_updated');
          this.router.navigate(['/ordering/execution-proposal', this.proposalId]);
        },
        error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
      });
    } else {
      const request: SendExecutionProposalRequest = {
        comment: this.proposalForm.value.comment,
        orderId: this.orderId!,
        receiverId: this.receiverId!
      };
      this.executionProposalService.sendExecutionProposal(request).subscribe({
        next: () => {
          this.toastHelper.showSuccess('common.success', 'ordering.execution_proposal_sent');
          this.router.navigate(['/ordering'], { queryParams: { tab: 0, subTab: 1 } });
        },
        error: (error: HttpErrorResponse) => this.errorHandler.handleApiError(error)
      });
    }
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;
}