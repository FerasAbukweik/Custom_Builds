import { Component, Input } from '@angular/core';
import { IStep } from './order-review.model';
import { IOrderDTO } from '../../my-orders.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-review',
  imports: [CommonModule],
  templateUrl: './order-review.component.html',
  host: {
    class:
      'group bg-deep-blue/50 border border-dark-gray/20 rounded-2xl p-4 md:p-6 lg:p-8 pb-8 shadow-xl w-full',
  },
})
export class OrderReviewComponent {
  @Input({ required: true }) order!: IOrderDTO;
  steps: IStep[] = [
    { id: 1, label: 'DESIGN CONFIRMED', date: 'Oct 10', status: 'completed', icon: 'fa-check' },
    { id: 2, label: 'IN ASSEMBLY', date: 'In Progress', status: 'current', icon: 'fa-tools' },
    { id: 3, label: 'TESTING', date: 'Pending', status: 'upcoming', icon: 'fa-microchip' },
    { id: 4, label: 'SHIPPED', date: 'Pending', status: 'upcoming', icon: 'fa-truck' },
  ];

  isStepActive(stepId: number): boolean {
    if (this.order.progress === 100) return true;
    if (this.order.progress === 75) return stepId <= 3;
    if (this.order.progress === 50) return stepId <= 2;
    if (this.order.progress === 25) return stepId <= 1;
    return false;
  }
}
