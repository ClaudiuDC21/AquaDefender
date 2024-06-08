import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IconService } from '../../utils/services/icon.service';

@Component({
  selector: 'app-alert-success',
  templateUrl: './alert-success.component.html',
  styleUrl: './alert-success.component.scss',
})
export class AlertSuccessComponent implements OnInit {
  @Input() message: string = '';
  @Output() close = new EventEmitter<void>();

  constructor(public iconService: IconService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.onClose();
    }, 5000);
  }

  onClose() {
    this.close.emit();
  }
}
