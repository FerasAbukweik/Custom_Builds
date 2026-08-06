import { inject, Injectable, signal } from '@angular/core';
import { ProductApiService } from '../api-services/product-api-service';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { IProductDTO } from '../../DTO/product-dto';

@Injectable({ providedIn: 'root' })
export class ProductService {
  // DI
  private readonly _productApiService = inject(ProductApiService);

  // signals
  private _products = signal<IProductDTO[]>([]);
  private _isLoading = signal<boolean>(false);

  // fields

  // private
  private _isMoreDataAvaiable = true;
  private readonly _lazyData: ILazyDTO = {
    sectionSize: 10,
    taken: 0,
  };

  // getters
  get products() {
    return this._products.asReadonly();
  }

  get isLoading() {
    return this._isLoading.asReadonly();
  }

  // methods

  // lazyGetProducts
  lazyGetProducts = () => {
    if (this._isLoading() || !this._isMoreDataAvaiable) return;
    this._isLoading.set(true);

    this._productApiService.getAll(this._lazyData).subscribe({
      next: (res) => {
        this._products.update((curr) => [...curr, ...res]);

        this._lazyData.taken += res.length;
        this._isMoreDataAvaiable = res.length > 0;
        this._isLoading.set(false);

        console.log('Products:', this._products());
      },
      error: (err) => {
        this._isLoading.set(false);
      },
    });
  };

  remove(productId: string) {
    // old data used to return to old data
    const oldData = this._products();

    this._products.update((curr) => curr.filter((p) => p.id !== productId));

    this._productApiService.remove(productId).subscribe({
      error: () => {
        // TODO: show error

        // return to old data
        this._products.set(oldData);
      },
    });
  }
}
