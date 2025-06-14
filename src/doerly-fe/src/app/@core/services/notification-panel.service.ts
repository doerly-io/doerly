import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationPanelService {
  private isVisibleSubject = new BehaviorSubject<boolean>(false);
  public isVisible$ = this.isVisibleSubject.asObservable();

  show(): void {
    this.isVisibleSubject.next(true);
  }

  hide(): void {
    this.isVisibleSubject.next(false);
  }

  toggle(): void {
    this.isVisibleSubject.next(!this.isVisibleSubject.value);
  }

  open(): void {
    this.isVisibleSubject.next(true);
  }

  close(): void {
    this.isVisibleSubject.next(false);
  }
} 