import { CurrencyPipe } from '@angular/common';
import { Component, DestroyRef, inject, OnInit, Signal, signal } from '@angular/core';
import { TopNavComponent } from '../../layouts/top-nav.component/top-nav.component';
import { RouterLink } from '@angular/router';
import { CartItemServices } from '../../core/services/cart-item-services';
import { __importDefault } from 'tslib';
import { ICartItemDTO } from '../../core/DTO/cart-item-dto';
import { ILazyGetCartItemsDTO } from '../../core/DTO/lazy-get-cart-items-dto';
import { catchError, debounceTime, firstValueFrom, map, of, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { INewQuantities } from './cart.model';
import { IsVisableDirective } from '../../shared/directives/is-visable.directive';
import { LoadingComponent } from "../../shared/components/loading/loading.component/loading.component";
import { ICartSummaryInfo } from '../../core/DTO/cart-summary-info-dto';

@Component({
  selector: 'app-cart',
  imports: [CurrencyPipe, TopNavComponent, RouterLink, CurrencyPipe, IsVisableDirective, LoadingComponent],
  templateUrl: './cart.component.html',
})
export class CartComponent implements OnInit {
  // injections
  private readonly cartItemService = inject(CartItemServices);
  private readonly destroyRef = inject(DestroyRef);

  // signals
  cartItems = signal<ICartItemDTO[]>([]);
  newQuantities = signal<INewQuantities>({});
  isLoading = signal<boolean>(true);

  // observables
  // use observable for debounce time
  newQuantities$ = toObservable(this.newQuantities);

  // fields

  // priavte
  private requestData: ILazyGetCartItemsDTO = {
    Section: 0,
    ElementsPerSection: 10,
  };
  private isMoreDataAvailable : boolean = true;
  
  // public
  summaryInfo = signal<ICartSummaryInfo>({
    shippingCost: 0,
    tax: 0,
    totalOrders: 0,
    totalPrice: 0
  });

  ngOnInit(): void {
    // get inital cart items
    this.lazyGetCartItems(true);

    // get summary info
    this.getSummaryInfo();

    // track quantities changes
    this.trackQuantitiesChanges();
  }

  trackQuantitiesChanges(){
    // we need this for later if something went wrong and we need to restore the previous state
    let prevItems : ICartItemDTO[] = [];
    let prevSummary : ICartSummaryInfo | undefined = undefined;

    // check for quantities changes
    this.newQuantities$.pipe(
      // thats for optimisitic update
      tap((val) => {
      // only the first time we need backup so we dont loose data
      if(prevItems.length === 0) prevItems = this.cartItems();
      if(!prevSummary) prevSummary = this.summaryInfo();

      // update quantity immediately for better user experience
      const idsSet = new Set(Object.keys(val));

      let operation : number = 0;
      let itemPrice : number = 0;

      this.cartItems.update(curr =>
        curr.map(item =>{
          if(idsSet.has(item.id)){
            operation = val[item.id] - item.quantity;
            itemPrice = item.totalPrice;
            return {...item, quantity: val[item.id]};
          }
          else{
            return item;
          }
        }));


      this.summaryInfo.update(curr => ({
        ...curr, 
        totalOrders: curr.totalOrders + operation,
        totalPrice: curr.totalPrice + (operation * itemPrice)
      }));
      
    }) ,
    debounceTime(500) , takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: async (newQuantities : INewQuantities) => {
        if(!(await this.updateItemsQuantity(newQuantities))){
          // TODO: show error message
          // restore previous items
          this.cartItems.set(prevItems);
          this.summaryInfo.set(prevSummary!);
        }

        prevItems = [];
        prevSummary = undefined;
      }
    });
  }

  getSummaryInfo(){
    this.cartItemService.GetSummaryInfo().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (res) => {
        this.summaryInfo.set(res);
      },
      error: (err) => {
        // toDo: show error message
      }
    });
  }

  lazyGetCartItems(skipIsLoading : boolean = false){
    if(!this.isMoreDataAvailable || (!skipIsLoading && this.isLoading())) return;
    if(!skipIsLoading) this.isLoading.set(true);

    this.cartItemService.GetCartItems(this.requestData)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: (res) => {
        this.cartItems.update(curr => ([...curr, ...res]));

        this.requestData.Section++;
        
        this.isMoreDataAvailable = res.length > 0;
        this.isLoading.set(false);
      },
      error: (err) => {
        // toDo: show error message
        if(err.error.status === 404){
          this.isMoreDataAvailable = false;
        }

        this.isLoading.set(false);
      }
    });
  }

  async updateItemsQuantity(newQuantities: INewQuantities): Promise<boolean> {
  try
  {
    await firstValueFrom(
      this.cartItemService.updateQuantity(newQuantities).pipe(takeUntilDestroyed(this.destroyRef))
    );
    return true;
  }
  catch (error) {
    return false;
  }
}

  Remove(id : string) {
    // to get back to previous state if something went wrong
    const prevItems = this.cartItems();
    const prevSummary = this.summaryInfo();

    let itemPrice = 0;

    this.cartItems.update(curr => curr.filter(ci => {
      if(ci.id === id){
        itemPrice = ci.totalPrice
        return false;
      }

      return true;
    }));

    
    // remove item from newQuantities signal
    this.newQuantities.update(curr => {
      const { [id]: _, ...rest } = curr;
      return rest;
    });

    this.summaryInfo.update(curr => ({
      ...curr, 
      totalOrders: curr.totalOrders - 1, 
      totalPrice: curr.totalPrice - itemPrice}));

    this.cartItemService.remove(id)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      error: () => {
        // if something went wrong return to previous data
        // toDo: show error message
        this.cartItems.set(prevItems);
      },
    });
  }

  updateQuantity(id : string , add : number) {
    const originalQuantity: number = this.cartItems().find(i => i.id === id)?.quantity!;

    const newVal = (this.newQuantities()[id] ?? originalQuantity) + add;

    if(newVal <= 0){
      this.Remove(id);
      return;
    }
     
    this.newQuantities.update(curr => ({...curr, [id]: newVal}));
  }
}
