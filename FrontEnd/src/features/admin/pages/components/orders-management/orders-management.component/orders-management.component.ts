import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { stateCardData } from '../../../../../../core/interfaces/state-card-data';
import { OrdersManagementService } from './orders-management.service';
import { StateDataComponent } from '../../../../../../shared/components/stateData/state-data.component';
import { OrdersTableComponent } from '../../../../../../shared/components/orders-table/orders-table.component';
import { OrderDetailsDialogComponent } from '../../../../../../shared/components/order-details-dialog/order-details-dialog.component';

@Component({
  selector: 'app-orders-management.component',
  imports: [StateDataComponent, OrdersTableComponent, OrderDetailsDialogComponent],
  providers: [OrdersManagementService],
  templateUrl: './orders-management.component.html',
  host: {
    class: 'bg-primary text-off-white min-h-screen font-display w-full flex',
  },
})
export class OrdersManagementComponent implements OnInit {
  // DI
  protected readonly ordersManagementService = inject(OrdersManagementService);

  // signals
  protected statusData = computed<stateCardData>(() => [
    {
      name: 'Pending Orders',
      value: this.ordersManagementService.summary().pendingOrdersCount.toString(),
    },
    {
      name: 'New Orders',
      value: this.ordersManagementService.summary().latestOrdersCount.toString(),
    },
  ]);

  ngOnInit() {
    this.ordersManagementService.init();
  }
}
