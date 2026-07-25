import { inject, Injectable, signal } from '@angular/core';
import { CartItemApiServices } from '../api-services/cart-item-api-services';
import { IMiniCartItemDTO } from '../../DTO/mini-cart-item-dto';
import { ICartSummaryInfo } from '../../DTO/cart-summary-info-dto';
import { Id_Quantity_DTO, INewQuantities } from '../../../features/cart/cart.model';
import { catchError, debounceTime, firstValueFrom, map, of, switchMap, tap } from 'rxjs';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { Subject } from 'rxjs';
import { ICustomBuildAddDTO } from '../../DTO/add-custom-build-dto';
import { CustomBuildTypeEnum } from '../../enums/custom-build-type-enum';
import { OrderTypeEnum } from '../../enums/order-type-enum';

@Injectable({
  providedIn: 'root',
})
export class CartItemService {
  // injections
  private readonly cartItemApiService = inject(CartItemApiServices);

  // signals
  private cartItems = signal<IMiniCartItemDTO[]>([]);
  private isLoading = signal<boolean>(false);
  private isDeleteing = signal<boolean>(false);
  private summaryInfo = signal<ICartSummaryInfo>({
    shippingCost: 0,
    tax: 0,
    totalOrders: 0,
    totalPrice: 0,
  });

  // behaviroeasubject
  trackQuantities$ = new Subject<Id_Quantity_DTO>();

  // fields

  // private
  private newQuantities: INewQuantities = [];
  private isMoreDataAvailable: boolean = true;
  private requestData: ILazyDTO = {
    taken: 0,
    sectionSize: 10,
  };

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

  // constructor
  constructor() {
    this.trackQuantitiesChanges();
  }

  // methods

  // update summary info
  updateSummaryInfo = () => {
    this.cartItemApiService.GetSummaryInfo().subscribe({
      next: (res) => {
        this.summaryInfo.set(res);
      },
      error: (err) => {
        // toDo: show error message
      },
    });
  };

  // get cart items from api with lazy laoding
  lazyGetCartItems = async () => {
    if (!this.isMoreDataAvailable || this.isLoading()) return;
    this.isLoading.set(true);

    while (this.isDeleteing()) {
      await new Promise<void>((res) => setTimeout(res, 250));
    }

    this.cartItemApiService.GetCartItems(this.requestData).subscribe({
      next: (data) => {
        this.cartItems.update((curr) => [...curr, ...data]);

        this.requestData.taken += data.length;
        this.isMoreDataAvailable = data.length > 0;
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      },
    });
  };

  // remove cart item
  removeCartItem = (id: string) => {
    if (this.isDeleteing() || this.isLoading()) return;
    this.isDeleteing.set(true);
    // to get back to previous state if something went wrong
    const prevItems = this.cartItems();
    const prevSummary = this.summaryInfo();

    let itemPrice: number = 0;
    let quantity: number = 0;

    this.cartItems.update((curr) =>
      curr.filter((ci) => {
        if (ci.id === id) {
          itemPrice = ci.price;
          quantity = ci.quantity;
          return false;
        }

        return true;
      }),
    );

    this.summaryInfo.update((curr) => ({
      ...curr,
      totalOrders: curr.totalOrders - quantity,
      totalPrice: curr.totalPrice - itemPrice * quantity,
    }));

    this.cartItemApiService.remove(id).subscribe({
      next: () => {
        this.requestData.taken--;
        this.isDeleteing.set(false);
      },
      error: () => {
        this.cartItems.set(prevItems);
        this.summaryInfo.set(prevSummary);
        this.isDeleteing.set(false);
      },
    });
  };

  addCustomBuild(customBuildData: ICustomBuildAddDTO) {
    this.cartItemApiService.addCustomBuild(customBuildData).subscribe();
  }

  addProduct(productId: string) {
    this.cartItemApiService.addProduct(productId).subscribe();
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

          this.newQuantities = this.newQuantities.map((nq) => {
            if (nq.itemId === newQ.itemId) {
              found = true;
              return { ...nq, newQuantity: newQ.newQuantity };
            }
            return nq;
          });

          if (!found) {
            this.newQuantities.push(newQ);
          }

          // only the first time we need backup so we dont loose data
          if (!prevItems) prevItems = this.cartItems();
          if (!prevSummary) prevSummary = this.summaryInfo();

          let operation: number = 0; // -1 or 1
          let itemPrice: number = 0;

          // generate new cart items
          const newCartItems: IMiniCartItemDTO[] = this.cartItems().map((item) => {
            if (item.id == newQ.itemId) {
              operation = newQ.newQuantity - item.quantity;
              itemPrice = item.price;
              return { ...item, quantity: newQ.newQuantity };
            }
            return item;
          });

          // generate new summary
          const newSummaryInfo: ICartSummaryInfo = {
            ...this.summaryInfo(),
            totalOrders: this.summaryInfo().totalOrders + operation,
            totalPrice: this.summaryInfo().totalPrice + operation * itemPrice,
          };

          // optimistic update for better ux
          this.cartItems.set(newCartItems);
          this.summaryInfo.set(newSummaryInfo);
        }),
        debounceTime(500),
        switchMap(() => {
          return this.cartItemApiService.updateQuantity(this.newQuantities);
        }),
      )
      .subscribe({
        next: async () => {
          // reset everything for the next round
          prevItems = null;
          prevSummary = null;
          this.newQuantities = [];
        },
        error: () => {
          this.cartItems.set(prevItems!);
          this.summaryInfo.set(prevSummary!);

          prevItems = null;
          prevSummary = null;
          this.newQuantities = [];
        },
      });
  }
}
