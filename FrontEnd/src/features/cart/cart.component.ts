import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { __importDefault } from 'tslib';
import { Id_Quantity_DTO, INewQuantities } from './cart.model';
import { CartItemService } from '../../core/services/client-services/cart-item-service-service';
import { CurrencyPipe } from '@angular/common';
import { LoadingComponent } from '../../shared/components/loading/loading.component';
import { RouterLink } from '@angular/router';
import { TopNavComponent } from '../../layouts/top-nav/top-nav.component';
import { IsVisableDirective } from '../../shared/directives/is-visable.directive';

@Component({
  selector: 'app-cart',
  imports: [CurrencyPipe, LoadingComponent, RouterLink, TopNavComponent, IsVisableDirective],
  templateUrl: './cart.component.html',
})
export class CartComponent implements OnInit {
  // injections
  protected readonly cartService = inject(CartItemService);

  // protected
  protected summaryInfo = this.cartService.summaryInfo;

  ngOnInit(): void {
    // update summary info
    this.cartService.updateSummaryInfo();
  }

  // methods

  // update quantity
  updateQuantity(id: string, newQuantity: number) {
    if (newQuantity <= 0) {
      this.cartService.removeCartItem(id);
      return;
    }

    const newQ: Id_Quantity_DTO = {
      itemId: id,
      newQuantity: newQuantity,
    };

    this.cartService.trackQuantities$.next(newQ);
  }
}
