import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { Card } from 'primeng/card';
import { SendExecutionProposalRequest } from '../../models/requests/send-execution-proposal-request';
import { ExecutionProposalService } from '../../domain/execution-proposal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { setServerErrors } from 'app/@core/helpers/input-validation-helpers';
import { ButtonDirective } from 'primeng/button';

@Component({
  selector: 'app-send-execution-proposal',
  imports: [
    ReactiveFormsModule,
    Card,
    TranslatePipe,
    ButtonDirective
  ],
  templateUrl: './send-execution-proposal.component.html',
  styleUrl: './send-execution-proposal.component.scss'
})
export class SendExecutionProposalComponent {
  sendExecutionProposalForm!: FormGroup;
  orderId!: number;
  receiverId!: number;

  constructor(private formBuilder: FormBuilder,
    private executionProposalService: ExecutionProposalService,
    private router: Router,
    private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.orderId = +params['orderId'];
      this.receiverId = +params['receiverId'];
    });
   this.initForm();
  }

  initForm(): void {
    this.sendExecutionProposalForm = this.formBuilder.group({
      comment: ['']
    });
  }

  sendExecutionProposal(): void {
      const request = this.sendExecutionProposalForm.value as SendExecutionProposalRequest;
      request.senderId = 2;
      request.orderId = this.orderId;
      request.receiverId = this.receiverId;
  
      this.executionProposalService.sendExecutionProposal(request).subscribe({
        next: (value) => {
          this.router.navigate(['/ordering'], { queryParams: { tab: 0, subTab: 1 } });
        },
        error: (error: HttpErrorResponse) => {
          if (error.status === 400) {
            const errors = error.error.errors;
            setServerErrors(this.sendExecutionProposalForm, errors);
          }
        }
      });
    }
}
