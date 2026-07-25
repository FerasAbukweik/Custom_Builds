import { computed, DestroyRef, inject, Injectable, signal } from '@angular/core';
import { OrderApiService } from '../../../../../core/services/api-services/order-api-service';
import { ILazyDTO } from '../../../../../core/DTO/lazy-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IHistoryOrderDTO } from '../../../../../core/DTO/History-orders-dto';
import { firstValueFrom } from 'rxjs';
import { IOrderHistorySummaryDTO } from '../../../../../core/DTO/order-history-dto';

@Injectable({ providedIn: 'root' })
export class HistoryService {
  // injections
  private readonly _orderService = inject(OrderApiService);

  // signals
  private _currentSection = signal<number>(1);
  private _SectionOrders = signal<IHistoryOrderDTO[][]>([]);
  private _isLoading = signal<boolean>(false);
  private _summary = signal<IOrderHistorySummaryDTO>({
    count: 0,
    totalPrice: 0,
  });

  // computed
  private _currOrders = computed(() => this._SectionOrders()[this._currentSection() - 1] ?? []);
  private _numberOfSections = computed(() => this._SectionOrders().length);

  // fields

  // private
  private _sectionSize = 10;

  // getters
  get getSectionOrders() {
    return this._SectionOrders.asReadonly();
  }

  get getNumberOfSections() {
    return this._numberOfSections;
  }

  get getCurrOrders() {
    return this._currOrders;
  }

  get getIsLoading() {
    return this._isLoading.asReadonly();
  }

  get getSummary() {
    return this._summary.asReadonly();
  }

  get getCurrentSection() {
    return this._currentSection.asReadonly();
  }

  // mehtods

  async init() {
    try {
      const summaryData = await firstValueFrom(this._orderService.getHistorySummary());

      this._summary.set(summaryData);

      const numberOfSections = Math.ceil(summaryData.count / this._sectionSize);
      const oldNumberOfSections = this._SectionOrders().length;

      if (!oldNumberOfSections) {
        // if first time make all indexes empty so we fitch data for all of them
        this._SectionOrders.set(Array(numberOfSections));
      } else {
        if (numberOfSections > oldNumberOfSections) {
          // so we have new place in the array for the new section
          this._SectionOrders.update((curr) => [
            ...curr,
            ...Array(numberOfSections - oldNumberOfSections),
          ]);
        } else {
          // remove data from last index so if new items where added we have the chance to check it
          const temp = this._SectionOrders();
          temp[numberOfSections - 1] = Array();
          this._SectionOrders.set(temp);
        }
      }

      if (numberOfSections > 0) this.changeSection(1);
    } catch (error) {
      // toDO: show error
    }
  }

  changeSection = async (section: number) => {
    section--; // to deal with 0 indexing
    if (this._SectionOrders()[section] && this._SectionOrders()[section].length) {
      this._currentSection.set(section + 1);
      return;
    }
    this._isLoading.set(true);

    const lazyData: ILazyDTO = {
      taken: section * this._sectionSize,
      sectionSize: this._sectionSize,
    };

    this._orderService.lazyGetCompletedOrders(lazyData).subscribe({
      next: (data) => {
        this._SectionOrders.update((curr) => {
          const next = [...curr];
          next[section] = data;
          return next;
        });

        this._currentSection.set(section + 1);
        this._isLoading.set(false);
      },
      error: () => {
        this._isLoading.set(false);
      },
    });
  };

  // buy again
  buyAgain = (orderId: string) => {
    this._orderService.buyAgain(orderId).subscribe({
      next: () => {
        // toDO: show message
      },
      error: () => {
        // toDo: show message
      },
    });
  };
}
