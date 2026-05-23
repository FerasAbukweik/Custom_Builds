import { DestroyRef, inject, Injectable, signal } from '@angular/core';
import { OrderService } from '../../../../../core/services/api-services/order-service';
import { ILazyLoadingDTO } from '../../../../../core/DTO/lazy-loading-dto';
import { ILazyGetAllOrdersDTO } from '../../../../../core/DTO/lazy-get-all-orders-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IHistoryOrderDTO } from '../../../../../core/DTO/History-orders-dto';
import { IOrderHistory } from './history.component.model';
import { IOrderHistoryDTO } from '../../../../../core/DTO/order-history-dto';

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

  get getTotalSpent(){
    return this._totalSpent.asReadonly();
  }


  // mehtods
  updateOrders = async (section: number) => {
    section--;
    if (this._SectionOrders()[section] && this._SectionOrders()[section].length){
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
    this._orderService.getHistorySummary()
    .pipe(this._untilDestroyed)
    .subscribe({
        next: (data) =>{
            const res = data as IOrderHistoryDTO;
            this._completedOrdersCount.set(res.count);
            
            const numberOfSections = Math.ceil(res.count / this._lazyLoadingData.ElementsPerSection)
            const oldNumberOfSections = this._SectionOrders().length;

            if(!oldNumberOfSections){
                // if first time make all indexes empty so we fitch data for all of them
                this._SectionOrders.set(Array(numberOfSections));
                this._totalSpent.set(res.totalPrice);
            }
            else{
                if(numberOfSections > oldNumberOfSections){
                    // so we have new place in the array for the new section
                    this._SectionOrders.update(curr => [...curr , Array()])
                }
                else{
                    // do this so if new items was added we have the chance to check it
                    const temp = this._SectionOrders();
                    temp[numberOfSections - 1] = Array();
                    this._SectionOrders.set(temp);
                }
            }

            if(numberOfSections > 0) this.updateOrders(1);
        },
        error: () => {
            // toDO: show error
        }
    })
  }


  // buy again
  buyAgain = (orderId: string) => {
    this._orderService.buyAgain(orderId).subscribe({
        next: ()=>{
            // toDO: show message
        },
        error: ()=>{
            // toDo: show message
        }
    });
  }
}
