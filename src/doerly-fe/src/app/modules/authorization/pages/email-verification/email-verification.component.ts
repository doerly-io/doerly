import {Component, OnInit} from '@angular/core';
import {AuthService} from 'app/modules/authorization/domain/auth.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-email-verification',
  imports: [],
  templateUrl: './email-verification.component.html',
  styleUrl: './email-verification.component.scss'
})
export class EmailVerificationComponent implements OnInit {

  constructor(private readonly quthService: AuthService,
              private readonly route: ActivatedRoute,
              private readonly router: Router,
  ) {
  }


  ngOnInit(): void {
    const params = this.route.snapshot.queryParams;

    const token: string = params['token'];
    const email: string = params['email'];

    if (token && email) {
      this.verifyEmail(token, email);
    } else {
      this.router.navigate(['/404-page']);
    }
  }

  verifyEmail(token: string, email: string): void {
    this.quthService.verifyEmail(token, email).subscribe({
      next: (response) => {

        this.router.navigate(['/login']);
      },
      error: error => {
        this.router.navigate(['/404-page']);

      }
    });

  }


}
