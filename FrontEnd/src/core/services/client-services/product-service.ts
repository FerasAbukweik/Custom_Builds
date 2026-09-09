import { inject, Injectable, signal } from '@angular/core';
import { ProductApiService } from '../api-services/product-api-service';
import { ILazyDTO } from '../../DTO/lazy-dto';
import { IProductDTO } from '../../DTO/product-dto';
import { ProductAddDTO } from 'src/core/DTO/product-add-dto';
import { ProductEditDTO } from 'src/core/DTO/product-edit-dto';

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

  add(productData: ProductAddDTO) {
    // old data used to return to old data
    const oldData = this._products();

    const tempId = Date.now().toString();

    // optimistic update
    this._products.update((curr) => [
      ...curr,
      {
        id: tempId,
        title: productData.name,
        price: productData.price,
        image: productData.images.length > 0 ? URL.createObjectURL(productData.images[0]) : '',
        stock: productData.inStock,
        description: productData.description,
      },
    ]);

    // call api
    this._productApiService.add(productData).subscribe({
      next: (newId) => {
        // update the id of the new product
        this._products.update((curr) =>
          curr.map((p) => (p.id === tempId ? { ...p, id: newId } : p)),
        );
      },
      error: () => {
        // TODO: show error

        // return to old data
        this._products.set(oldData);
      },
    });
  }

  edit(editData: ProductEditDTO) {
    // old data used to return to old data
    const oldData = this._products();
    let oldProduct = oldData.find((p) => p.id === editData.id);

    if (!oldProduct) return;

    // optimistic update
    this._products.update((curr) =>
      curr.map((p) =>
        p.id === editData.id
          ? {
              ...p,
              description: editData.description ?? p.description,
              price: editData.price ?? p.price,
              stock: editData.inStock ?? p.stock,
              title: editData.name ?? p.title,
            }
          : p,
      ),
    );

    // only send what needs to be updated
    editData.name = editData.name === oldProduct.title ? null : editData.name;
    editData.description =
      editData.description === oldProduct.description ? null : editData.description;
    editData.price = editData.price === oldProduct.price ? null : editData.price;
    editData.inStock = editData.inStock === oldProduct.stock ? null : editData.inStock;

    // if nothing is changed stop the request
    if (
      editData.name === null &&
      editData.description === null &&
      editData.price === null &&
      editData.inStock === null
    )
      return;

    // call api
    this._productApiService.edit(editData).subscribe({
      error: () => {
        // TODO: show error

        // return to old data
        this._products.set(oldData);
      },
    });
  }
}
