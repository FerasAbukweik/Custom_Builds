import { CurrencyPipe } from '@angular/common';
import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { TopNavComponent } from '../../layouts/top-nav.component/top-nav.component';
import { RouterLink } from '@angular/router';
import { __importDefault } from 'tslib';
import { ICartItemDTO } from '../../core/DTO/cart-item-dto';
import { debounceTime, tap } from 'rxjs';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { INewQuantities } from './cart.model';
import { IsVisableDirective } from '../../shared/directives/is-visable.directive';
import { LoadingComponent } from '../../shared/components/loading/loading.component/loading.component';
import { ICartSummaryInfo } from '../../core/DTO/cart-summary-info-dto';
import { CartItemGlobalService } from '../../core/services/global-services/cart-item-global-service';

@Component({
  selector: 'app-cart',
  imports: [
    CurrencyPipe,
    TopNavComponent,
    RouterLink,
    CurrencyPipe,
    IsVisableDirective,
    LoadingComponent,
  ],
  templateUrl: './cart.component.html',
})
export class CartComponent implements OnInit {
  // injections
  private readonly destroyRef = inject(DestroyRef);
  private readonly cartService = inject(CartItemGlobalService);

  // signals
  newQuantities = signal<INewQuantities>({});
  cartItems = this.cartService.getCartItems;
  summaryInfo = this.cartService.getSummaryInfo;
  isLoading = this.cartService.getIsLoading;

  // observables
  // use observable for debounce time
  newQuantities$ = toObservable(this.newQuantities);

  ngOnInit(): void {
    // get inital cart items
    this.cartService.lazyGetCartItems();

    // get summary info
    this.cartService.updateSummaryInfo();

    // track quantities changes
    this.trackQuantitiesChanges();
  }


  // lazy get cart items
  get lazyGetCartItems() {
    return this.cartService.lazyGetCartItems();
  }



  // track quantities changes
  trackQuantitiesChanges() {
    // we need this for later if something went wrong and we need to restore the previous state
    let prevItems: ICartItemDTO[] = [];
    let prevSummary: ICartSummaryInfo | undefined = undefined;

    // check for quantities changes
    this.newQuantities$
      .pipe(
        // thats for optimisitic update
        tap((val) => {
          // only the first time we need backup so we dont loose data
          if (prevItems.length === 0) prevItems = this.cartItems();
          if (!prevSummary) prevSummary = this.summaryInfo();

          // using set for o(1) checks
          const idsSet = new Set(Object.keys(val));

          let operation: number = 0; // -1 or 1
          let itemPrice: number = 0;

          const newCartItems: ICartItemDTO[] = this.cartItems().map((item) => {
            if (idsSet.has(item.id)) {
              operation = this.newQuantities()[item.id] - item.quantity;
              itemPrice = item.totalPrice;
              return { ...item, quantity: this.newQuantities()[item.id] };
            }
            return item;
          });

          const newSummaryInfo: ICartSummaryInfo = {
            ...this.summaryInfo(),
            totalOrders: this.summaryInfo().totalOrders + operation,
            totalPrice: this.summaryInfo().totalPrice + (operation * itemPrice),
          };

          // optimistic update for better ux
          this.cartService.setCartItems(newCartItems);
          this.cartService.setSummartInfo(newSummaryInfo);
        }),
        debounceTime(500),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: async (newQuantities: INewQuantities) => {
          if (!(await this.cartService.updateItemsQuantity(newQuantities))) {
            // TODO: show error message
            // restore previous items
            this.cartService.setCartItems(prevItems);
            this.cartService.setSummartInfo(prevSummary!);
          }

          prevItems = [];
          prevSummary = undefined;
        },
      });
  }


  // update quantity
  updateQuantity(id: string, add: number) {
    const originalQuantity: number = this.cartItems().find((i) => i.id === id)?.quantity!;

    const newVal = (this.newQuantities()[id] ?? originalQuantity) + add;

    if (newVal <= 0) {
      this.removeCartItem(id);
      return;
    }

    this.newQuantities.update((curr) => ({ ...curr, [id]: newVal }));
  }


  // remove cart item
  removeCartItem(id: string) {
    this.cartService.removeCartItem(id);
  }
}
