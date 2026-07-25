import { inject, Injectable, signal } from '@angular/core';
import { OrderApiService } from '../api-services/order-api-service';
import { IOrderDTO } from '../../DTO/mini-order-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';

@Injectable({ providedIn: 'root' })
export class OrderService {
  // injections
  private readonly orderApiService = inject(OrderApiService);

  // signals
  private completedOrders = signal<IOrderDTO[]>([]);
  private isLoadingCompletedOrders = signal<boolean>(false);
  private completedOrdersCount = signal<number>(0);

  //fields

  // private
  private _lazyData: ILazyDTO = {
    sectionSize: 10,
    taken: 0,
  };
  private isMoreDataAvailable: boolean = true;

  // getters
  get getCompletedOrders() {
    return this.completedOrders.asReadonly();
  }

  get getIsLoadingCompletedOrders() {
    return this.isLoadingCompletedOrders.asReadonly();
  }

  get getCompletedOrdersCount() {
    return this.completedOrdersCount.asReadonly();
  }

  lazyGetOrders = () => {
    if (!this.isMoreDataAvailable || this.isLoadingCompletedOrders()) return;
    this.isLoadingCompletedOrders.set(true);

    this.orderApiService.getAllProcessingOrders(this._lazyData).subscribe({
      next: (data) => {
        this.completedOrders.update((curr) => [...curr, ...data]);

        const dataLen = data.length;

        this._lazyData.taken += dataLen;
        this.isMoreDataAvailable = dataLen > 0;
        this.isLoadingCompletedOrders.set(false);
      },
      error: () => {
        this.isLoadingCompletedOrders.set(false);
      },
    });
  };

  updateOrdersCount = () => {
    this.orderApiService.getProcessingOrdersCount().subscribe({
      next: (res) => {
        this.completedOrdersCount.set(res);
      },
      error: () => {
        // toDo: show error message
      },
    });
  };
}
