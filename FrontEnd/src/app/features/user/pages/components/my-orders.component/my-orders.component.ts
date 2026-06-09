import { Component, inject, OnInit } from '@angular/core';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { OrderReviewComponent } from './components/order-review.component/order-review.component';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component';
import { IsVisableDirective } from '../../../../../shared/directives/is-visable.directive';
import { MyOrdersService } from './my-orders.service';

@Component({
  selector: 'app-my-orders',
  imports: [UserContentWrapper, OrderReviewComponent, LoadingComponent, IsVisableDirective],
  templateUrl: './my-orders.component.html',
  host: {
    class: 'h-full',
  },
})
export class MyOrdersComponent implements OnInit {
  // injections
  private readonly _myOrdersService = inject(MyOrdersService);

  // signals
  orders = this._myOrdersService.getOrders;
  isLoading = this._myOrdersService.getIsLoading;
  OrdersCount = this._myOrdersService.getOrdersCount;

  ngOnInit(): void {
    // update orders Count
    this._myOrdersService.updateOrdersCount();

    // so we check for new orders every time we open it again
    this._myOrdersService.setIsMoreDataAvaiable(true);
  }

  // methods
  lazyGetOrders = this._myOrdersService.lazyGetOrders;
}
