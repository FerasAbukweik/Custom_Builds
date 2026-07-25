import { Component, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { HistoryService } from './history.service';
import { StateDataComponent } from '../../../../../shared/components/stateData/state-data.component';
import { OrdersTableComponent } from '../../../../../shared/components/orders-table/orders-table.component';

@Component({
  selector: 'app-history',
  imports: [CommonModule, UserContentWrapper, StateDataComponent, OrdersTableComponent],
  templateUrl: './history.component.html',
})
export class HistoryComponent implements OnInit {
  // injections
  protected readonly historyService = inject(HistoryService);

  // signals
  protected statCardData = computed(() => {
    return [
      { name: 'Total Spent', value: '$' + this.historyService.getSummary().totalPrice.toFixed(2) },
      { name: 'Total Orders', value: this.historyService.getSummary().count.toString() },
    ];
  });

  // methods

  // onInit
  async ngOnInit() {
    await this.historyService.init();
  }
}
