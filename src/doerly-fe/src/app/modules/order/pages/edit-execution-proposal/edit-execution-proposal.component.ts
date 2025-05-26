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
    NgIf
  ]
})
export class EditExecutionProposalComponent implements OnInit {
  proposalForm!: FormGroup;
  proposalId?: number;
  orderId?: number;
  receiverId?: number;
  isEdit: boolean = false;
  loading: boolean = true;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private executionProposalService: ExecutionProposalService,
    private toastHelper: ToastHelper
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
        error: (error: HttpErrorResponse) => {
          if (error.status === 400) {
            this.toastHelper.showError('common.error', error.error.errorMessage);
          }
          else {
            this.toastHelper.showError('common.error', 'common.error-occurred');
          }
        }
      });
    } else {
      this.loading = false;
      this.orderId = Number(this.route.snapshot.queryParamMap.get('orderId'));
      this.receiverId = Number(this.route.snapshot.queryParamMap.get('receiverId'));
    }
  }

  initForm(): void {
    this.proposalForm = this.formBuilder.group({
      comment: ['', [Validators.maxLength(1000)]]
    });
  }

  submit(): void {
    if (this.proposalForm.invalid) return;

    if (this.isEdit) {
      const updatedProposal: UpdateExecutionProposalRequest = {
        id: this.proposalId!,
        comment: this.proposalForm.value.comment
      };
      this.executionProposalService.updateExecutionProposal(updatedProposal).subscribe({
        next: () => {
          this.router.navigate(['/ordering/execution-proposal', this.proposalId]);
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
    } else {
      const request: SendExecutionProposalRequest = {
        comment: this.proposalForm.value.comment,
        orderId: this.orderId!,
        receiverId: this.receiverId!
      };
      this.executionProposalService.sendExecutionProposal(request).subscribe({
        next: () => {
          this.router.navigate(['/ordering'], { queryParams: { tab: 0, subTab: 1 } });
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
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;
}