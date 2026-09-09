import { Injectable, signal } from '@angular/core';
import { ProductDialogComponent } from './product-dialog.component';
import { IProductDTO } from 'src/core/DTO/product-dto';

@Injectable()
export class ProductDialogService {
  private _component: ProductDialogComponent | null = null;

  // signals
  private _product = signal<IProductDTO | null>(null);

  get product() {
    return this._product;
  }

  register(component: ProductDialogComponent) {
    this._component = component;
  }

  open(product: IProductDTO | null = null) {
    if (!this._component) return;

    this._product.set(product);
    this._component.open();
  }

  close() {
    if (!this._component) return;

    this._component.close();
  }
}
