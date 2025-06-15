import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationPanelService {
  private toggleSubject = new Subject<Event>();
  public toggle$ = this.toggleSubject.asObservable();

  toggle(event: Event): void {
    this.toggleSubject.next(event);
  }
}
