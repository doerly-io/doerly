import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExecutionProposalService } from '../../domain/execution-proposal.service';
import { GetExecutionProposalResponse } from '../../models/responses/get-execution-proposal-response';
import { UpdateExecutionProposalRequest } from '../../models/requests/update-execution-proposal-request';
import { HttpErrorResponse } from '@angular/common/http';
import { setServerErrors, getError, isInvalid, getServersideError } from 'app/@core/helpers/input-validation-helpers';
import { Card } from 'primeng/card';
import { ButtonDirective } from 'primeng/button';
import { TranslatePipe } from '@ngx-translate/core';
import { NgIf } from '@angular/common';

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
  editProposalForm!: FormGroup;
  proposalId!: number;
  loading = true;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private executionProposalService: ExecutionProposalService
  ) {}

  ngOnInit(): void {
    this.proposalId = Number(this.route.snapshot.paramMap.get('id'));
    this.initForm();
    this.loadProposal();
  }

  initForm(): void {
    this.editProposalForm = this.fb.group({
      comment: ['', [Validators.maxLength(1000)]]
    });
  }

  loadProposal(): void {
    this.executionProposalService.getExecutionProposal(this.proposalId).subscribe({
      next: (response) => {
        const proposal = response.value as GetExecutionProposalResponse;
        if (proposal) {
          this.editProposalForm.patchValue({
            comment: proposal.comment
          });
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  updateProposal(): void {
    if (this.editProposalForm.invalid) return;
    const updatedProposal: UpdateExecutionProposalRequest = {
      id: this.proposalId,
      comment: this.editProposalForm.value.comment
    };
    this.executionProposalService.updateExecutionProposal(updatedProposal).subscribe({
      next: () => {
        this.router.navigate(['/ordering/execution-proposal', this.proposalId]);
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 400) {
          setServerErrors(this.editProposalForm, error.error.errors);
        }
      }
    });
  }

  protected readonly getError = getError;
  protected readonly isInvalid = isInvalid;
  protected readonly getServersideError = getServersideError;
}
