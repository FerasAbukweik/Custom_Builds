import { Component, input, signal } from '@angular/core';
import { IProductDTO } from '../../../../core/DTO/product-dto';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product-card',
  imports: [CurrencyPipe],
  standalone: true,
  templateUrl: './product-card.component.html',
})
export class ProductCardComponent {
  // signals
  productData = input.required<IProductDTO>();
}
