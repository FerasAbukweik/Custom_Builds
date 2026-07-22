import { computed, DestroyRef, inject, Injectable, signal } from '@angular/core';
import { OrderService } from '../../../../../core/services/api-services/order-service';
import { ILazyLoadingDTO } from '../../../../../core/DTO/lazy-loading-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IHistoryOrderDTO } from '../../../../../core/DTO/History-orders-dto';
import { ILazyGetUserDataDTO } from '../../../../../core/DTO/lazy-get-user-data-dto';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class HistoryService {
  // injections
  private readonly _orderService = inject(OrderService);
  private readonly _destroyRef = inject(DestroyRef);

  // signals
  private _SectionOrders = signal<IHistoryOrderDTO[][]>([]);
  private _isLoading = signal<boolean>(false);
  private _completedOrdersCount = signal<number>(-1);
  private _currentSection = signal<number>(1);
  private _totalSpent = signal<number>(0);

  // computed
  private _currOrders = computed(() => this._SectionOrders()[this._currentSection() - 1] ?? []);
  private _numberOfSections = computed(() => this._SectionOrders().length);

  // fields

  // private
  private _lazyLoadingData: ILazyLoadingDTO = {
    taken: 0,
    ElementsPerSection: 10,
  };

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

  get getCompletedOrdersCount() {
    return this._completedOrdersCount.asReadonly();
  }

  get getCurrentSection() {
    return this._currentSection.asReadonly();
  }

  get getTotalSpent() {
    return this._totalSpent.asReadonly();
  }

  // mehtods
  changeSection = async (section: number) => {
    section--; // to deal with 0 indexing
    if (this._SectionOrders()[section] && this._SectionOrders()[section].length) {
      this._currentSection.set(section + 1);
      return;
    }
    this._isLoading.set(true);

    const requestData: ILazyGetUserDataDTO = {
      taken: section * this._lazyLoadingData.ElementsPerSection,
      ElementsPerSection: this._lazyLoadingData.ElementsPerSection,
    };

    this._orderService
      .getAllCompletedOrders(requestData)
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
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

  // init SectionOrders
  init = async () => {
    try {
      const data = await firstValueFrom(this._orderService.getHistorySummary());

      this._completedOrdersCount.set(data.count);

      const numberOfSections = Math.ceil(data.count / this._lazyLoadingData.ElementsPerSection);
      const oldNumberOfSections = this._SectionOrders().length;

      if (!oldNumberOfSections) {
        // if first time make all indexes empty so we fitch data for all of them
        this._SectionOrders.set(Array(numberOfSections));
        this._totalSpent.set(data.totalPrice);
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
