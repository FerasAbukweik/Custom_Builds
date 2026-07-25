import { DestroyRef, inject, Injectable, signal } from '@angular/core';
import { ProductApiService } from '../api-services/product-api-service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { IProductDTO } from '../../DTO/product-dto';

@Injectable({ providedIn: 'root' })
export class ProductService {
  // DI
  private readonly productApiService = inject(ProductApiService);

  // signals
  private products = signal<IProductDTO[]>([]);
  private isLoading = signal<boolean>(false);

  // fields

  // private
  private isMoreDataAvaiable = true;
  private readonly lazyData: ILazyDTO = {
    sectionSize: 10,
    taken: 0,
  };

  // getters
  get getProducts() {
    return this.products.asReadonly();
  }

  get getIsLoading() {
    return this.isLoading.asReadonly();
  }

  // methods

  // lazyGetProducts
  lazyGetProducts = () => {
    if (this.isLoading() || !this.isMoreDataAvaiable) return;
    this.isLoading.set(true);

    this.productApiService.getAll(this.lazyData).subscribe({
      next: (res) => {
        this.products.update((curr) => [...curr, ...res]);

        this.lazyData.taken += res.length;
        this.isMoreDataAvaiable = res.length > 0;
        this.isLoading.set(false);
      },
      error: (err) => {
        this.isLoading.set(false);
      },
    });
  };
}
