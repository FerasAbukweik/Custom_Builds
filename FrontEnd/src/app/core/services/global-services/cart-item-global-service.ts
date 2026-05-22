import { DestroyRef, inject, Injectable, signal } from '@angular/core';
import { CartItemServices } from '../api-services/cart-item-services';
import { ICartItemDTO } from '../../DTO/cart-item-dto';
import { ICartSummaryInfo } from '../../DTO/cart-summary-info-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ILazyGetCartItemsDTO } from '../../DTO/lazy-get-cart-items-dto';
import { INewQuantities } from '../../../features/cart.component/cart.model';
import { catchError, firstValueFrom, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CartItemGlobalService {
  // injections
  private readonly cartItemService = inject(CartItemServices);
  private readonly destroyRef = inject(DestroyRef);

  // signals
  private cartItems = signal<ICartItemDTO[]>([]);
  private isLoading = signal<boolean>(false);
  private isDeleteing = signal<boolean>(false);
  private summaryInfo = signal<ICartSummaryInfo>({
    shippingCost: 0,
    tax: 0,
    totalOrders: 0,
    totalPrice: 0,
  });

  // fields

  // private
  private isMoreDataAvailable: boolean = true;
  private requestData: ILazyGetCartItemsDTO = {
    taken: 0,
    ElementsPerSection: 10,
  };
  private readonly untilDestroyed = takeUntilDestroyed(this.destroyRef);

  // getters
  get getCartItems() {
    return this.cartItems.asReadonly();
  }

  get getSummaryInfo() {
    return this.summaryInfo.asReadonly();
  }

  get getIsLoading() {
    return this.isLoading.asReadonly();
  }


  // setters
  setCartItems = (newCartItem: ICartItemDTO[]) => {
    this.cartItems.set(newCartItem);
  }

  setSummartInfo = (newSummaryInfo: ICartSummaryInfo) => {
    this.summaryInfo.set(newSummaryInfo);
  }

  setIsMoreDataAvaiable = (newVal: boolean) => {
    this.isMoreDataAvailable = newVal;
  }
  
  // methods

  // update summary info
  updateSummaryInfo = () => {
    this.cartItemService.GetSummaryInfo()
      .pipe(this.untilDestroyed)
      .subscribe({
        next: (res) => {
          this.summaryInfo.set(res as ICartSummaryInfo);
        },
        error: (err) => {
          // toDo: show error message
        },
      });
  }

  // get cart items from api with lazy laoding
  lazyGetCartItems = async ()  => {
    if (!this.isMoreDataAvailable || this.isLoading()) return;
    this.isLoading.set(true);

    while(this.isDeleteing()){
      await new Promise<void>(res => setTimeout(res , 250))
    }

    this.cartItemService.GetCartItems(this.requestData)
      .pipe(this.untilDestroyed)
      .subscribe({
        next: (res) => {
          this.cartItems.update((curr) => [...curr, ...(res as ICartItemDTO[])]);

          const itemsLen = (res as ICartItemDTO[]).length;
          console.log("test");
          console.log(res as ICartItemDTO[]);

          this.requestData.taken += itemsLen;
          this.isMoreDataAvailable = itemsLen > 0;
          this.isLoading.set(false);
        },
        error: (err) => {
          // toDo: show error message
          if (err.error.status === 404) {
            this.isMoreDataAvailable = false;
          }

          this.isLoading.set(false);
        },
      });
  }

  // update items quantity
  updateItemsQuantity = async (newQuantities: INewQuantities): Promise<boolean> => {
    try {
      await firstValueFrom(
        this.cartItemService.updateQuantity(newQuantities)
          .pipe(this.untilDestroyed),
      );
      return true;
    } catch (error) {
      return false;
    }
  }

  // remove cart item
  removeCartItem = async (id: string): Promise<boolean> => {
    if(this.isDeleteing() || this.isLoading()) return false;
    this.isDeleteing.set(true);
    // to get back to previous state if something went wrong
    const prevItems = this.cartItems();
    const prevSummary = this.summaryInfo();

    let itemPrice : number = 0;
    let quantity : number = 0;

    this.cartItems.update((curr) =>
      curr.filter((ci) => {
        if (ci.id === id) {
          itemPrice = ci.totalPrice;
          quantity = ci.quantity;
          return false;
        }

        return true;
      }),
    );

    this.summaryInfo.update((curr) => ({
      ...curr,
      totalOrders: curr.totalOrders - quantity,
      totalPrice: curr.totalPrice - (itemPrice * quantity),
    }));

    return await firstValueFrom(
      this.cartItemService.remove(id)
      .pipe(
        this.untilDestroyed,
        map(() => {
          this.requestData.taken--;
          this.isDeleteing.set(false);
          return true;
        }),
        catchError(() => {
          // TODO: show error message
          this.cartItems.set(prevItems);
          this.summaryInfo.set(prevSummary);
          this.isDeleteing.set(false);
          return of(false);
        }),
      ),
    );
  }
}
