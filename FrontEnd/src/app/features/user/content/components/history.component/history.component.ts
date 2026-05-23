import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { IOrderHistory, IStatsData } from './history.component.model';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { UserContentWrapper } from '../../wrappers/user-content.wrapper/user-content.wrapper';
import { HistoryService } from './history.service';
import { OrderStateEnum } from '../../../../../core/enums/order-status-enum';
import { LoadingComponent } from '../../../../../shared/components/loading/loading.component/loading.component';


@Component({
  selector: 'app-history',
  imports: [CommonModule, UserContentWrapper , DatePipe , CurrencyPipe , LoadingComponent],
  templateUrl: './history.component.html',
})
export class HistoryComponent implements OnInit {
  // injections
  private readonly _historyService = inject(HistoryService);

  // signals
  currentSection = this._historyService.getCurrentSection;
  completedOrdersCount = this._historyService.getCompletedOrdersCount;
  SectionOrders = this._historyService.getSectionOrders;
  isLoading = this._historyService.getIsLoading;
  totalSpent = this._historyService.getTotalSpent;

  // computed
  currOrders = computed(() => this.SectionOrders()[this.currentSection() - 1])

  // fields

  ngOnInit(): void {
    this._historyService.init();
  }


  // methods
  generateBottomNumbers = (maxLen: number = 3) : number[] => {
    const numberOfSections = this.SectionOrders().length;

    const takeRight = Math.min(maxLen - 1 , numberOfSections - this.currentSection());
    const takeLeft = Math.min(maxLen - takeRight - 1,  this.currentSection() - 1);

    let begin = this.currentSection() - takeLeft;
    const finalLen = 1 + takeRight + takeLeft;
    const resArr = Array.from({ length: finalLen }, () => begin++);

    return resArr;
  }


  // change section
  changeSection(newSection: number){
    if(this.currentSection() === newSection) return;
    if(newSection < 1 || newSection > this.SectionOrders().length) return;

    this._historyService.updateOrders(newSection);
  }

  // get status name
  getStatusName = (status: OrderStateEnum) => {
    return OrderStateEnum[status];
  } 

  // buyAgain
  buyAgain = this._historyService.buyAgain;
}