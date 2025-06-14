import { Component } from '@angular/core';
import {NgOptimizedImage} from '@angular/common';
import {Card} from 'primeng/card';

@Component({
  selector: 'app-home',
  imports: [
    NgOptimizedImage,
    Card
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
