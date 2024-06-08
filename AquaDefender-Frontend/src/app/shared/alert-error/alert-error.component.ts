import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IconService } from '../../utils/services/icon.service';

@Component({
  selector: 'app-alert-error',
  templateUrl: './alert-error.component.html',
  styleUrl: './alert-error.component.scss',
})
export class AlertErrorComponent {
  @Input() message: string = '';
  @Output() close = new EventEmitter<void>();

  constructor(public iconService: IconService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.onClose();
    }, 10000);
  }

  onClose() {
    this.close.emit();
  }
}
