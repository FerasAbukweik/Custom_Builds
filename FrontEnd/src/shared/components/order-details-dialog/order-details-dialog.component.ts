import { Component, ElementRef, inject, signal, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderDetailsDialogService } from './order-detaild-dialog-service';
import { OrderItemService } from '../../../core/services/client-services/order-item-service';
import { OrderService } from '../../../core/services/client-services/order-service';
import { OrderStateEnum } from '../../../core/enums/order-status-enum';
import { IsVisableDirective } from '../../directives/is-visable.directive';
import { LoadingComponent } from '../loading/loading.component';
import { OrderTypeEnum } from 'src/core/enums/order-type-enum';

@Component({
  selector: 'dialog[app-order-details-dialog]',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe, IsVisableDirective, LoadingComponent],
  templateUrl: './order-details-dialog.component.html',
  host: {
    class:
      'fixed inset-0 z-100 m-auto bg-transparent p-4 backdrop:backdrop-blur-md backdrop:bg-primary/50 open:flex items-center justify-center border-none outline-none max-w-none max-h-none w-full h-full',
    '(click)': 'closeModal()',
  },
})
export class OrderDetailsDialogComponent {
  // DI
  private readonly _elementRef = inject<ElementRef<HTMLDialogElement>>(ElementRef);
  protected readonly orderItemService = inject(OrderItemService);
  protected readonly orderService = inject(OrderService);
  protected readonly orderDetailsDialogService = inject(OrderDetailsDialogService);

  // signals
  protected selectedStatus = signal<number>(0);

  // computed
  protected orderItems = computed(
    () =>
      this.orderItemService.orderItems()[this.orderDetailsDialogService.selectedOrderId()] ?? [],
  );
  protected orderDetails = computed(
    () => this.orderService.ordersDetails()[this.orderDetailsDialogService.selectedOrderId()] ?? {},
  );

  // getters

  get orderStatus() {
    return Object.keys(OrderStateEnum).filter((k) => isNaN(Number(k)));
  }

  // methods
  ngOnInit() {
    this.orderDetailsDialogService.register(this);
  }

  getStatusString(state: OrderStateEnum) {
    return OrderStateEnum[state];
  }

  getOrderType(type: OrderTypeEnum) {
    return OrderTypeEnum[type];
  }

  // Open Native HTML Dialog
  public openModal(): void {
    this._elementRef.nativeElement.showModal();
    this.orderService.fetchOrderDetails(this.orderDetailsDialogService.selectedOrderId());
    console.log('testing');
  }

  // Close Native HTML Dialog
  public closeModal(): void {
    this._elementRef.nativeElement.close();
  }

  public updateStatus(): void {
    const newStatus = this.selectedStatus();
  }

  public viewItem(itemId: string): void {
    // Logic to view individual item details
  }
}
