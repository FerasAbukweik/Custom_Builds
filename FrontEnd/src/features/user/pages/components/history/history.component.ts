import { Component, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { HistoryService } from './history.service';
import { OrdersTableComponent } from "src/shared/components/orders-table/orders-table.component";
import { StateDataComponent } from "src/shared/components/stateData/state-data.component";

@Component({
  selector: 'app-history',
  imports: [CommonModule, UserContentWrapper, OrdersTableComponent, StateDataComponent],
  providers: [HistoryService],
  templateUrl: './history.component.html',
})
export class HistoryComponent implements OnInit {
  // injections
  protected readonly historyService = inject(HistoryService);

  // signals
  protected statCardData = computed(() => {
    return [
      { name: 'Total Spent', value: '$' + this.historyService.summary().totalPrice.toFixed(2) },
      { name: 'Total Orders', value: this.historyService.summary().count.toString() },
    ];
  });

  // methods

  // onInit
  async ngOnInit() {
    await this.historyService.init();
  }
}
