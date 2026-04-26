import { Component } from '@angular/core';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { IOrder } from './my-orders.model';
import { OrderReviewComponent } from './components/order-review.component/order-review.component';

@Component({
  selector: 'app-my-orders',
  imports: [UserContentWrapper, OrderReviewComponent],
  templateUrl: './my-orders.component.html',
})
export class MyOrdersComponent {
  orders: IOrder[] = [
    {
      id: 'CP-8842',
      name: "'Neo-Tokyo' Limited Edition",
      category: 'Custom Keyboard',
      image: 'assets/images/keyboard-image.png',
      status: 'In Assembly',
      deliveryDate: 'Oct 24, 2023',
      progress: 50,
    },
    {
      id: 'CP-9901',
      name: 'Pro-Grip Wireless Mouse',
      category: 'Gaming Mouse',
      image: 'assets/images/keyboard-image.png',
      status: 'Design Confirmed',
      deliveryDate: 'Nov 02, 2023',
      progress: 25,
    },
  ];
}
