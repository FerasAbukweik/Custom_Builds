import { DestroyRef, inject, Injectable, signal } from '@angular/core';
import { OrderService } from '../../../../../core/services/api-services/order-service';
import { ILazyLoadingDTO } from '../../../../../core/DTO/lazy-loading-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IOrderDTO } from '../../../../../core/DTO/mini-order-dto';

@Injectable({ providedIn: 'root' })
export class MyOrdersService {
  // injections
  private readonly _orderService = inject(OrderService);
  private readonly _destroyRef = inject(DestroyRef);

  // signals
  private orders = signal<IOrderDTO[]>([]);
  private isLoading = signal<boolean>(false);
  private ordersCount = signal<number>(0);

  //fields

  // private
  private _lazyData: ILazyLoadingDTO = {
    ElementsPerSection: 10,
    taken: 0,
  };
  private isMoreDataAvailable: boolean = true;
  private untilDestroyed = takeUntilDestroyed(this._destroyRef);

  // getters
  get getOrders() {
    return this.orders.asReadonly();
  }

  get getIsLoading() {
    return this.isLoading.asReadonly();
  }

  get getOrdersCount() {
    return this.ordersCount.asReadonly();
  }

  lazyGetOrders = () => {
    if (!this.isMoreDataAvailable || this.isLoading()) return;
    this.isLoading.set(true);

    this._orderService
      .getAllProcessingOrders(this._lazyData)
      .pipe(this.untilDestroyed)
      .subscribe({
        next: (data) => {
          this.orders.update((curr) => [...curr, ...(data as IOrderDTO[])]);

          const dataLen = (data as IOrderDTO[]).length;

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
  };

  updateOrdersCount = () => {
    this._orderService
      .getProcessingOrdersCount()
      .pipe(this.untilDestroyed)
      .subscribe({
        next: (res) => {
          this.ordersCount.set(res as number);

          console.log(res);
        },
        error: () => {
          // toDo: show error message
        },
      });
  };
}
