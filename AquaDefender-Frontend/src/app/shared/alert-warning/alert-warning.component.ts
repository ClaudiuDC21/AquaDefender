import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IconService } from '../../utils/services/icon.service';

@Component({
  selector: 'app-alert-warning',
  templateUrl: './alert-warning.component.html',
  styleUrl: './alert-warning.component.scss',
})
export class AlertWarningComponent {
  @Input() message: string = '';
  @Output() close = new EventEmitter<void>();

  constructor(public iconService: IconService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.onClose();
    }, 15000);
  }

  onClose() {
    this.close.emit();
  }
}
