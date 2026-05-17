import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { IOrderDTO } from './my-orders.model';
import { OrderReviewComponent } from './components/order-review.component/order-review.component';
import { OrderService } from '../../../../../core/services/order-service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ILazyLoadingDTO } from '../../../../../core/DTO/lazy-loading-dto';
import { LoadingComponent } from "../../../../../shared/components/loading/loading.component/loading.component";
import { IsVisableDirective } from '../../../../../shared/directives/is-visable.directive';

@Component({
  selector: 'app-my-orders',
  imports: [UserContentWrapper, OrderReviewComponent, LoadingComponent , IsVisableDirective],
  templateUrl: './my-orders.component.html',
  host:{
    class: "h-full"
  }
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
    Section: 0,
  };
  private isMoreDataAvailable : boolean = true;

  ngOnInit(): void {
    // get initial orders
    this.lazyGetOrders(false);
  }


  lazyGetOrders(checkIsLoading: boolean = true){
    if(!this.isMoreDataAvailable || (checkIsLoading && this.isLoading())) return;
    this.isLoading.set(true);

    this._orderService
      .GetAll(this._lazyData)
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: (data) => {
          this.orders.update(curr => [...curr, ...data]);
          this._lazyData.Section++;

          console.log(data);

          this.isMoreDataAvailable = data.length > 0;
        },
        error: (err) => {
          console.log(err.error);
          this.isMoreDataAvailable = false;
        },
        complete: () => {
          this.isLoading.set(false);
        }
      });
  }
}
