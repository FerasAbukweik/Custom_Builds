import { inject, Injectable, signal } from '@angular/core';
import { OrderApiService } from '../api-services/order-api-service';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { IOrderDto } from '../../DTO/orders-dto';
import { OrderDetailsDto } from '../../DTO/order-details-dto';

@Injectable({ providedIn: 'root' })
export class OrderService {
  // injections
  private readonly _orderApiService = inject(OrderApiService);

  // signals
  private _processingOrders = signal<IOrderDto[]>([]);
  private _isLoadingCompletedOrders = signal<boolean>(false);
  private _processingOrdersCount = signal<number>(-1);
  private _ordersDetails = signal<Record<string, OrderDetailsDto>>({});
  private _isLoadingOrderDetails = signal<boolean>(false);

  //fields

  // private
  private _lazyData: ILazyDTO = {
    sectionSize: 10,
    taken: 0,
  };
  private _isMoreDataAvailable: boolean = true;

  // getters
  get processingOrders() {
    return this._processingOrders.asReadonly();
  }

  get isLoadingProcessingOrders() {
    return this._isLoadingCompletedOrders.asReadonly();
  }

  get processingOrdersCount() {
    return this._processingOrdersCount.asReadonly();
  }

  get ordersDetails() {
    return this._ordersDetails.asReadonly();
  }

  get isLoadingOrderDetails() {
    return this._isLoadingOrderDetails.asReadonly();
  }

  lazyGetProcessingOrders = () => {
    if (!this._isMoreDataAvailable || this._isLoadingCompletedOrders()) return;
    this._isLoadingCompletedOrders.set(true);

    this._orderApiService.LazyGetProcessingOrders(this._lazyData).subscribe({
      next: (data) => {
        this._processingOrders.update((curr) => [...curr, ...data]);

        const dataLen = data.length;

        this._lazyData.taken += dataLen;
        this._isMoreDataAvailable = dataLen > 0;
        this._isLoadingCompletedOrders.set(false);
      },
      error: () => {
        this._isLoadingCompletedOrders.set(false);
      },
    });
  };

  updateOrdersCount = () => {
    this._orderApiService.getProcessingOrders().subscribe({
      next: (res) => {
        this._processingOrdersCount.set(res);
      },
      error: () => {
        // toDo: show error message
      },
    });
  };

  fetchOrderDetails(orderId: string) {
    if (this._ordersDetails()[orderId] || !orderId) return;
    this._isLoadingOrderDetails.set(true);

    this._orderApiService.getOrderDetails(orderId).subscribe({
      next: (data) => {
        this._ordersDetails.update((curr) => ({
          ...curr,
          [orderId]: data,
        }));

        this._isLoadingOrderDetails.set(false);
      },
      error: () => {
        this._isLoadingOrderDetails.set(false);
      },
    });
  }
}
