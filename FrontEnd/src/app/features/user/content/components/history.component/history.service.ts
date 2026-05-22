import { DestroyRef, inject, Injectable, signal } from '@angular/core';
import { OrderService } from '../../../../../core/services/api-services/order-service';
import { ILazyLoadingDTO } from '../../../../../core/DTO/lazy-loading-dto';
import { ILazyGetAllOrdersDTO } from '../../../../../core/DTO/lazy-get-all-orders-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IHistoryOrderDTO } from '../../../../../core/DTO/History-orders-dto';

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

  // fields

  // private
  private _lazyLoadingData: ILazyLoadingDTO = {
    taken: 0,
    ElementsPerSection: 10,
  };
  private readonly _untilDestroyed = takeUntilDestroyed(this._destroyRef);


  // getters
  get getSectionOrders(){
    return this._SectionOrders.asReadonly();
  }

  get getIsLoading(){
    return this._isLoading.asReadonly();
  }

  get getCompletedOrdersCount(){
    return this._completedOrdersCount.asReadonly();
  }

  get getCurrentSection(){
    return this._currentSection.asReadonly();
  }


  // mehtods
  updateOrders = async (section: number) => {
    section--;
    if (this._SectionOrders()[section]){
        this._currentSection.set(section + 1);
        return;
    }
    this._isLoading.set(true);

    const requestData: ILazyGetAllOrdersDTO = {
      taken: section * this._lazyLoadingData.ElementsPerSection,
      ElementsPerSection: this._lazyLoadingData.ElementsPerSection,
    };

    this._orderService
      .getAllCompletedOrders(requestData)
      .pipe(this._untilDestroyed)
      .subscribe({
        next: (data) => {
          const data2 = data as IHistoryOrderDTO[];

          this._SectionOrders.update((curr) => {
            const next = [...curr]
            next[section] = data2;
            return next;
        });

          this._isLoading.set(false);
          this._currentSection.set(section + 1);
        },
        error: () => {
          this._isLoading.set(false);
        },
      });
  };



  // init SectionOrders
  init = () => {
    this._orderService.getCompletedOrdersCount()
    .pipe(this._untilDestroyed)
    .subscribe({
        next: (data) =>{
            const count = data as number;
            this._completedOrdersCount.set(count);
            
            const numberOfSections = Math.ceil(count / this._lazyLoadingData.ElementsPerSection)
            this._SectionOrders.set(Array(numberOfSections));

            if(numberOfSections > 0) this.updateOrders(1);
        },
        error: () => {
            // toDO: show error
        }
    })
  }
}
