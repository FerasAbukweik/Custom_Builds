import { inject, Injectable, signal } from '@angular/core';
import { CartItemApiServices } from '../api-services/cart-item-api-services';
import { IMiniCartItemDTO } from '../../DTO/mini-cart-item-dto';
import { ICartSummaryInfo } from '../../DTO/cart-summary-info-dto';
import { Id_Quantity_DTO, INewQuantities } from '../../../features/cart/cart.model';
import { debounceTime, switchMap, tap } from 'rxjs';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { Subject } from 'rxjs';
import { ICustomBuildAddDTO } from '../../DTO/add-custom-build-dto';

@Injectable({
  providedIn: 'root',
})
export class CartItemService {
  // injections
  private readonly _cartItemApiService = inject(CartItemApiServices);

  // signals
  private _cartItems = signal<IMiniCartItemDTO[]>([]);
  private _isLoading = signal<boolean>(false);
  private _isDeleteing = signal<boolean>(false);
  private _summaryInfo = signal<ICartSummaryInfo>({
    shippingCost: 0,
    tax: 0,
    totalOrders: 0,
    totalPrice: 0,
  });

  // behaviroeasubject
  trackQuantities$ = new Subject<Id_Quantity_DTO>();

  // fields

  // private
  private _newQuantities: INewQuantities = [];
  private _isMoreDataAvailable: boolean = true;
  private _lazyData: ILazyDTO = {
    taken: 0,
    sectionSize: 10,
  };

  // getters
  get cartItems() {
    return this._cartItems.asReadonly();
  }

  get summaryInfo() {
    return this._summaryInfo.asReadonly();
  }

  get isLoading() {
    return this._isLoading.asReadonly();
  }

  // constructor
  constructor() {
    this.trackQuantitiesChanges();
  }

  // methods

  reset() {
    this._cartItems.set([]);
    this._isLoading.set(false);
    this._isDeleteing.set(false);
    this._summaryInfo.set({
      shippingCost: 0,
      tax: 0,
      totalOrders: 0,
      totalPrice: 0,
    });
    this._newQuantities = [];
    this._isMoreDataAvailable = true;
    this._lazyData.taken = 0;
  }

  // update summary info
  updateSummaryInfo = () => {
    this._cartItemApiService.GetSummaryInfo().subscribe({
      next: (res) => {
        this._summaryInfo.set(res);
      },
      error: (err) => {
        // toDo: show error message
      },
    });
  };

  // get cart items from api with lazy laoding
  lazyGetCartItems = async () => {
    if (!this._isMoreDataAvailable || this._isLoading()) return;
    this._isLoading.set(true);

    while (this._isDeleteing()) {
      await new Promise<void>((res) => setTimeout(res, 250));
    }

    this._cartItemApiService.GetCartItems(this._lazyData).subscribe({
      next: (data) => {
        this._cartItems.update((curr) => [...curr, ...data]);

        this._lazyData.taken += data.length;
        this._isMoreDataAvailable = data.length > 0;
        this._isLoading.set(false);
      },
      error: () => {
        this._isLoading.set(false);
      },
    });
  };

  // remove cart item
  removeCartItem = (id: string) => {
    if (this._isDeleteing() || this._isLoading()) return;
    this._isDeleteing.set(true);
    // to get back to previous state if something went wrong
    const prevItems = this._cartItems();
    const prevSummary = this._summaryInfo();

    let itemPrice: number = 0;
    let quantity: number = 0;

    this._cartItems.update((curr) =>
      curr.filter((ci) => {
        if (ci.id === id) {
          itemPrice = ci.price;
          quantity = ci.quantity;
          return false;
        }

        return true;
      }),
    );

    this._summaryInfo.update((curr) => ({
      ...curr,
      totalOrders: curr.totalOrders - quantity,
      totalPrice: curr.totalPrice - itemPrice * quantity,
    }));

    this._cartItemApiService.remove(id).subscribe({
      next: () => {
        this._lazyData.taken--;
        this._isDeleteing.set(false);
      },
      error: () => {
        this._cartItems.set(prevItems);
        this._summaryInfo.set(prevSummary);
        this._isDeleteing.set(false);
      },
    });
  };

  addCustomBuild(customBuildData: ICustomBuildAddDTO) {
    this._cartItemApiService.addCustomBuild(customBuildData).subscribe({
      next: (newItem) => {
        this._cartItems.update((curr) => [newItem, ...curr]);

        this._lazyData.taken++;
      },
    });
  }

  addProduct(productId: string) {
    this._cartItemApiService.addProduct(productId).subscribe({
      next: (newItem) => {
        this._cartItems.update((curr) => [newItem, ...curr]);

        this._lazyData.taken++;
      },
    });
  }

  // private

  // track quantities changes
  private trackQuantitiesChanges() {
    // we need this for later if something went wrong and we need to restore the previous state
    let prevItems: IMiniCartItemDTO[] | null = null;
    let prevSummary: ICartSummaryInfo | null = null;

    // check for quantities changes
    this.trackQuantities$
      .pipe(
        // thats for optimisitic update
        tap((newQ) => {
          // add the quantity update to the request qrray
          let found = false;

          this._newQuantities = this._newQuantities.map((nq) => {
            if (nq.itemId === newQ.itemId) {
              found = true;
              return { ...nq, newQuantity: newQ.newQuantity };
            }
            return nq;
          });

          if (!found) {
            this._newQuantities.push(newQ);
          }

          // only the first time we need backup so we dont loose data
          if (!prevItems) prevItems = this._cartItems();
          if (!prevSummary) prevSummary = this._summaryInfo();

          let operation: number = 0; // -1 or 1
          let itemPrice: number = 0;

          // generate new cart items
          const newCartItems: IMiniCartItemDTO[] = this._cartItems().map((item) => {
            if (item.id == newQ.itemId) {
              operation = newQ.newQuantity - item.quantity;
              itemPrice = item.price;
              return { ...item, quantity: newQ.newQuantity };
            }
            return item;
          });

          // generate new summary
          const newSummaryInfo: ICartSummaryInfo = {
            ...this._summaryInfo(),
            totalOrders: this._summaryInfo().totalOrders + operation,
            totalPrice: this._summaryInfo().totalPrice + operation * itemPrice,
          };

          // optimistic update for better ux
          this._cartItems.set(newCartItems);
          this._summaryInfo.set(newSummaryInfo);
        }),
        debounceTime(500),
        switchMap(() => {
          return this._cartItemApiService.updateQuantity(this._newQuantities);
        }),
      )
      .subscribe({
        next: async () => {
          // reset everything for the next round
          prevItems = null;
          prevSummary = null;
          this._newQuantities = [];
        },
        error: () => {
          this._cartItems.set(prevItems!);
          this._summaryInfo.set(prevSummary!);

          prevItems = null;
          prevSummary = null;
          this._newQuantities = [];
        },
      });
  }
}
