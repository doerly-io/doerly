import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AuthService} from '../../domain/auth.service';

@Component({
  selector: 'app-password-reset',
  imports: [],
  templateUrl: './password-reset.component.html',
  styleUrl: './password-reset.component.scss'
})
export class PasswordResetComponent implements OnInit {

  constructor(private route: ActivatedRoute,
              private router: Router,
              private authorizationService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const token = params['token'];
      if (token) {
        this.verifyToken(token);
      } else {
        this.router.navigate(['/404']);
      }
    });
  }

  verifyToken(token: string): void {



  }

}
