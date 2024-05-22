import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IconService } from '../../utils/icon.service';

@Component({
  selector: 'app-alert-info',
  templateUrl: './alert-info.component.html',
  styleUrl: './alert-info.component.scss'
})
export class AlertInfoComponent {
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
