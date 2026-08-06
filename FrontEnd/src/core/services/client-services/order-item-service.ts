import { inject, Injectable, signal } from '@angular/core';
import { OrderItemApiService } from '../api-services/order-item-api-service';
import { IOrderItemDto } from '../../DTO/order-item-dto';
import { ILazyDTO } from '../../DTO/lazy-dto';

@Injectable({ providedIn: 'root' })
export class OrderItemService {
  // DI
  private readonly _orderItemApiService = inject(OrderItemApiService);

  // signals                          orderId, order items
  private _ordersItems = signal<Record<string, IOrderItemDto[]>>({});
  private _isLoading = signal<boolean>(false);

  // private
  private _isCompleted: Record<string, boolean> = {};
  private _takenPerOrder: Record<string, number> = {};
  private _sectionSize = 10;

  // getters

  get orderItems() {
    return this._ordersItems.asReadonly();
  }

  get isLoading() {
    return this._isLoading.asReadonly();
  }

  // setters

  // methods

  lazyGetOrderItems(orderId: string) {
    if (this._isLoading() || this._isCompleted[orderId]) return;
    this._isLoading.set(true);

    const lazyData: ILazyDTO = {
      sectionSize: this._sectionSize,
      taken: this._takenPerOrder[orderId] ?? 0,
    };

    this._orderItemApiService.getOrderItems(orderId, lazyData).subscribe({
      next: (data) => {
        this._ordersItems.update((curr) => ({
          ...curr,
          [orderId]: [...(curr[orderId] ?? []), ...data],
        }));

        this._isLoading.set(false);
        this._isCompleted[orderId] = data.length == 0;
        this._takenPerOrder[orderId] = lazyData.taken + data.length;
      },
      error: () => {
        this._isLoading.set(false);
      },
    });
  }
}
