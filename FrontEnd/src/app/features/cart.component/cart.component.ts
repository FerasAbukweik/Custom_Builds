import { CurrencyPipe } from '@angular/common';
import { Component, DestroyRef, inject, OnInit, Signal, signal } from '@angular/core';
import { TopNavComponent } from '../../layouts/top-nav.component/top-nav.component';
import { RouterLink } from '@angular/router';
import { CartItemServices } from '../../core/services/cart-item-services';
import { __importDefault } from 'tslib';
import { ICartItemDTO } from '../../core/DTO/cart-item-dto';
import { ILazyGetCartItemsDTO } from '../../core/DTO/lazy-get-cart-items-dto';
import { debounceTime, switchMap } from 'rxjs';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { INewQuantities } from './cart.model';
import id from '@angular/common/locales/id';

@Component({
  selector: 'app-cart',
  imports: [CurrencyPipe, TopNavComponent, RouterLink , CurrencyPipe],
  templateUrl: './cart.component.html',
})
export class CartComponent implements OnInit {
  // injections
  private readonly cartItemService = inject(CartItemServices);
  private readonly destroyRef = inject(DestroyRef);

  // signals
  cartItems = signal<ICartItemDTO[]>([]);
  newQuantities = signal<INewQuantities>({});

  // observables
  newQuantities$ = toObservable(this.newQuantities);

  // fields
  private requestData: ILazyGetCartItemsDTO = {
    Section: 0,
    ElementsPerSection: 10,
  };

  ngOnInit(): void {
    // get inital cart items
    const sub = this.cartItemService.GetCartItems(this.requestData)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: (res) => {
        this.cartItems.update(curr => ([...curr, ...res]));

        this.requestData.Section++;
      },
      error: (err) => {
        console.log(err);
      },
    });


    // check for quantities changes
    const sub2 = this.newQuantities$.pipe(debounceTime(250) , takeUntilDestroyed(this.destroyRef))
    .subscribe({
      next: (newQuantities : INewQuantities) => {
        this.updateItemQuantity(newQuantities)
      }
    });
  }


  updateItemQuantity(newQuantities: INewQuantities) {
    // to get back to previous state if something went wrong
    const prevItems = this.cartItems();
    // set for o(1) time to check for ids
    const idsSet = new Set(Object.keys(newQuantities));
    
    // update quantity immediately for better user experience
    this.cartItems.update(curr =>
      curr.map(item => idsSet.has(item.id) ? {...item, quantity: newQuantities[item.id]} : item));

    // call api to update quantity
    const sub = this.cartItemService.updateQuantity(newQuantities)
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe({
      error: () => {
        // if something went wrong, return to previous data
        // toDo: show error message
        this.cartItems.set(prevItems);
      },
    });
  }

  updateQuantity(id : string , add : number) {
    const originalQuantity: number = this.cartItems().find(i => i.id === id)?.quantity!;

    this.newQuantities.update(curr => ({...curr, [id]: (curr[id] ?? originalQuantity) + add }));
  }



}
