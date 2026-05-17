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
  private requestData: ILazyGetCartItemsDTO = {
    Section: 0,
    ElementsPerSection: 10,
  };
  private isMoreDataAvailable : boolean = true;

  // getters

  get TotalPrice(){
    let totalPrice = 0;

    this.cartItems().forEach(item => {
      totalPrice += item.totalPrice * item.quantity;
  });

  return totalPrice;
}

get ItemsCount(){
  let itemsCount = 0;

  this.cartItems().forEach(item => {
    itemsCount += item.quantity;
  });

  return itemsCount;
}




  ngOnInit(): void {
    // get inital cart items
    this.lazyGetCartItems(true);

    // we need this for later if something goes wrong and we need to restore the previous state
    let prevItems : ICartItemDTO[] = [];

    // check for quantities changes
    this.newQuantities$.pipe(
      tap((val) => {

      // only the first time we need backup so we dont loose data
      if(prevItems.length === 0) prevItems = this.cartItems();

      // update quantity immediately for better user experience
      const idsSet = new Set(Object.keys(val));

      this.cartItems.update(curr =>
        curr.map(item => idsSet.has(item.id) ? {...item, quantity: val[item.id]!} : item));

    }) ,
    debounceTime(500) , takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: async (newQuantities : INewQuantities) => {
        if(!(await this.updateItemsQuantity(newQuantities))){
          // restore previous items
          this.cartItems.set(prevItems);
        }

        prevItems = [];
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
      },
      error: (err) => {
        // toDo: show error message
        if(err.error === "no items where found"){
          this.isMoreDataAvailable = false;
        }
      },
      complete: () => {
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

    this.cartItems.update(curr => curr.filter(ci => ci.id !== id));
    
    // remove item from newQuantities signal
    this.newQuantities.update(curr => {
      const { [id]: _, ...rest } = curr;
      return rest;
    });

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
