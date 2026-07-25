import { Component, inject } from '@angular/core';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { OrderReviewComponent } from './components/order-review.component/order-review.component';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { IsVisableDirective } from '../../../../../shared/directives/is-visable.directive';
import { OrderService } from '../../../../../core/services/client-services/order-service';

@Component({
  selector: 'app-my-orders',
  imports: [UserContentWrapper, OrderReviewComponent, LoadingComponent, IsVisableDirective],
  templateUrl: './my-orders.component.html',
  host: {
    class: 'h-full',
  },
})
export class MyOrdersComponent {
  // DI
  protected readonly orderService = inject(OrderService);
}
