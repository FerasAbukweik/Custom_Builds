import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { IOrderDTO } from './my-orders.model';
import { OrderReviewComponent } from './components/order-review.component/order-review.component';
import { OrderService } from '../../../../../core/services/api-services/order-service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ILazyLoadingDTO } from '../../../../../core/DTO/lazy-loading-dto';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component/loading.component';
import { IsVisableDirective } from '../../../../../shared/directives/is-visable.directive';

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
  private readonly _orderService = inject(OrderService);
  private readonly _destroyRef = inject(DestroyRef);

  // signals
  orders = signal<IOrderDTO[]>([]);
  isLoading = signal<boolean>(true);

  //fields
  private _lazyData: ILazyLoadingDTO = {
    ElementsPerSection: 10,
    taken: 0,
  };
  private isMoreDataAvailable: boolean = true;

  ngOnInit(): void {
    // get initial orders
    this.lazyGetOrders(false);
  }

  lazyGetOrders(checkIsLoading: boolean = true) {
    if (!this.isMoreDataAvailable || (checkIsLoading && this.isLoading())) return;
    this.isLoading.set(true);

    this._orderService
      .GetAll(this._lazyData)
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: (data) => {
          this.orders.update((curr) => [...curr, ...data]);

          const dataLen = data.length;

          this._lazyData.taken += dataLen;
          this.isMoreDataAvailable = dataLen > 0;
          this.isLoading.set(false);
        },
        error: (err) => {
          if (err.error.status === 404) {
            this.isMoreDataAvailable = false;
          }

          this.isLoading.set(false);
        },
      });
  }
}
