import { inject, Injectable, signal } from '@angular/core';
import { IOrderDto } from '../../../../../../core/DTO/orders-dto';
import { OrderManagementStatusDto } from '../../../../../../core/DTO/orders-management-status-dto';
import { AdminApiService } from '../../../../../../core/services/api-services/admin-api-service';
import { ILazyDTO } from '../../../../../../core/DTO/lazy-dto';
import { OrderStateEnum } from 'src/core/enums/order-status-enum';
import { OrderApiService } from 'src/core/services/api-services/order-api-service';

@Injectable({ providedIn: 'root' })
export class AdminOrdersService {
  // injections
  private readonly _adminApiService = inject(AdminApiService);
  private readonly _orderApiService = inject(OrderApiService);

  // signals
  private _SectionsOrders = signal<IOrderDto[][]>([]);
  private _isLoading = signal<boolean>(false);
  private _summary = signal<OrderManagementStatusDto>({
    latestOrdersCount: -1,
    pendingOrdersCount: -1,
  });

  // fields

  // private
  private _sectionSize = 10;

  // getters
  get sectionsOrders() {
    return this._SectionsOrders.asReadonly();
  }

  get isLoading() {
    return this._isLoading.asReadonly();
  }

  get summary() {
    return this._summary.asReadonly();
  }

  // mehtods

  updateStatus(orderId: string, newStatus: OrderStateEnum) {
    const sectionIdx = this._SectionsOrders().findIndex((x) => x.some((o) => o.id === orderId));
    const order = this._SectionsOrders()[sectionIdx].find((o) => o.id === orderId);

    if (!order || order.orderStatus === newStatus) return;

    this._orderApiService.updateStatus(orderId, newStatus).subscribe({
      next: () => {
        this._SectionsOrders.update((curr) =>
          curr.map((orderArr) =>
            orderArr.map((o) => (o.id === orderId ? { ...o, orderStatus: newStatus } : o)),
          ),
        );
      },
    });
  }

  async init() {
    this._adminApiService.getOrderManagementStatus().subscribe({
      next: (summaryData) => {
        this._summary.set(summaryData);

        const numberOfSections = Math.ceil(summaryData.pendingOrdersCount / this._sectionSize);

        this._SectionsOrders.set(Array(numberOfSections));

        if (numberOfSections > 0) this.loadSection(1);
      },
      error: () => {},
    });
  }

  loadSection = async (section: number) => {
    section--; // to deal with 0 indexing
    if (this._SectionsOrders()[section] && this._SectionsOrders()[section].length) {
      return;
    }
    this._isLoading.set(true);

    const lazyData: ILazyDTO = {
      taken: section * this._sectionSize,
      sectionSize: this._sectionSize,
    };

    this._adminApiService.lazyGetOrders(lazyData).subscribe({
      next: (data) => {
        this._SectionsOrders.update((curr) => {
          const next = [...curr];
          next[section] = data;
          return next;
        });

        this._isLoading.set(false);
      },
      error: () => {
        this._isLoading.set(false);
      },
    });
  };
}
