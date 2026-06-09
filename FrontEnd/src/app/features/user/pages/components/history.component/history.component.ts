import { Component, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { HistoryService } from './history.service';
import { stateCardData } from '../../../../../core/interceptors/state-card-data';
import { StateDataComponent } from '../../../../../shared/components/stateData/state-data.component';
import { OrdersTableComponent } from '../../../../../shared/components/orders-table/orders-table.component';

@Component({
  selector: 'app-history',
  imports: [CommonModule, UserContentWrapper, StateDataComponent, OrdersTableComponent],
  templateUrl: './history.component.html',
})
export class HistoryComponent implements OnInit {
  // injections
  private readonly _historyService = inject(HistoryService);

  // signals
  isLoading = this._historyService.getIsLoading;
  totalSpent = this._historyService.getTotalSpent;
  ordersCount = this._historyService.getCompletedOrdersCount;
  currentSection = this._historyService.getCurrentSection;

  // computed
  currOrders = this._historyService.getCurrOrders;
  numberOfSections = this._historyService.getNumberOfSections;

  // fields

  // public
  statCardData!: stateCardData;




  
  // methods

  // onInit
  async ngOnInit() {
    await this._historyService.init();

    this.statCardData = [
      { name: 'Total Spent', value: '$' + this.totalSpent().toFixed(2) },
      { name: 'Total Orders', value: this.ordersCount().toString() },
    ];
  }

  // change section
  changeSection = this._historyService.changeSection;

  // buyAgain
  buyAgain = this._historyService.buyAgain;
}
