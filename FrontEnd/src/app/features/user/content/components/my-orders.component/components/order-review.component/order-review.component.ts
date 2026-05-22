import { Component, input } from '@angular/core';
import { IStep } from './order-review.model';
import { IOrderDTO } from '../../../../../../../core/DTO/mini-order-dto';
import { CommonModule, DatePipe } from '@angular/common';
import { OrderStateEnum } from '../../../../../../../core/enums/order-status-enum';

@Component({
  selector: 'app-order-review',
  imports: [CommonModule , DatePipe],
  templateUrl: './order-review.component.html',
  host: {
    class:
      'group bg-deep-blue/50 border border-dark-gray/20 rounded-2xl p-4 md:p-6 lg:p-8 pb-8 shadow-xl w-full',
  },
})
export class OrderReviewComponent {
  // inputs
  order = input.required<IOrderDTO>() ;

  // data
  steps: IStep[] = [
    { id: 1, label: 'DESIGN CONFIRMED', date: 'Oct 10', status: 'completed', icon: 'fa-check' },
    { id: 2, label: 'IN ASSEMBLY', date: 'In Progress', status: 'current', icon: 'fa-tools' },
    { id: 3, label: 'TESTING', date: 'Pending', status: 'upcoming', icon: 'fa-microchip' },
    { id: 4, label: 'SHIPPED', date: 'Pending', status: 'upcoming', icon: 'fa-truck' },
  ];


  // methods
  isStepActive(stepId: number): boolean {
    if (this.order().status === OrderStateEnum.Completed) return true;
    if (this.order().status === OrderStateEnum.Testing) return stepId <= 3;
    if (this.order().status === OrderStateEnum.Processing) return stepId <= 2;
    if (this.order().status === OrderStateEnum.Shipped) return stepId <= 1;

    return false;
  }

  getStatusString(status: OrderStateEnum){
    return OrderStateEnum[status];
  }
}
