import { Component, inject, OnInit } from '@angular/core';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { OrderReviewComponent as OrderCardComponent } from './components/order-review.component/order-card.component';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { IsVisableDirective } from '../../../../../shared/directives/is-visable.directive';
import { OrderService } from '../../../../../core/services/client-services/order-service';

@Component({
  selector: 'app-my-orders',
  imports: [UserContentWrapper, OrderCardComponent, LoadingComponent, IsVisableDirective],
  templateUrl: './my-orders.component.html',
  host: {
    class: 'h-full',
  },
})
export class MyOrdersComponent implements OnInit {
  // DI
  protected readonly orderService = inject(OrderService);

  // methods

  ngOnInit(): void {
    this.orderService.updateOrdersCount();
  }
}
