import { Component, inject, input, signal } from '@angular/core';
import { IProductDTO } from '../../../../core/DTO/product-dto';
import { CurrencyPipe } from '@angular/common';
import { CartItemService } from '../../../../core/services/client-services/cart-item-service-service';

@Component({
  selector: 'app-product-card',
  imports: [CurrencyPipe],
  standalone: true,
  templateUrl: './product-card.component.html',
})
export class ProductCardComponent {
  // DI
  protected readonly cartItemService = inject(CartItemService);

  // input
  productData = input.required<IProductDTO>();
}
