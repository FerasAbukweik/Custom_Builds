import { CurrencyPipe } from '@angular/common';
import { Component } from '@angular/core';
import { TopNavComponent } from '../../shared/components/top-nav.component/top-nav.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-cart',
  imports: [CurrencyPipe, TopNavComponent, RouterLink],
  templateUrl: './cart.component.html',
})
export class CartComponent {
  cartItems = [1, 2, 3];
  specs = ['GATERON CLEAR', 'GATERON CLEAR', 'GATERON CLEAR'];
}
