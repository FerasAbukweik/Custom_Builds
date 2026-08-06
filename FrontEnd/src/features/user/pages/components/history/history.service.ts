import { inject, Injectable, signal } from '@angular/core';
import { OrderApiService } from '../../../../../core/services/api-services/order-api-service';
import { ILazyDTO } from '../../../../../core/DTO/lazy-dto';
import { IOrderDto } from '../../../../../core/DTO/orders-dto';
import { IOrderHistoryStatusDTO } from '../../../../../core/DTO/order-history-status-dto';

@Injectable()
export class HistoryService {
  // injections
  private readonly _orderService = inject(OrderApiService);

  // signals
  private _SectionsOrders = signal<IOrderDto[][]>([]);
  private _isLoading = signal<boolean>(false);
  private _summary = signal<IOrderHistoryStatusDTO>({
    count: 0,
    totalPrice: 0,
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

  async init() {
    this._orderService.getHistorySummary().subscribe({
      next: (summaryData) => {
        this._summary.set(summaryData);

        const numberOfSections = Math.ceil(summaryData.count / this._sectionSize);

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

    this._orderService.lazyGetOrders(lazyData).subscribe({
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
