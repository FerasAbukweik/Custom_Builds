import { Component, computed, input, output } from '@angular/core';
import { IHistoryOrderDTO } from '../../../core/DTO/History-orders-dto';
import { OrderStateEnum } from '../../../core/enums/order-status-enum';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { LoadingComponent } from "../loading/loading.component";

@Component({
  selector: 'app-orders-table',
  imports: [CurrencyPipe, DatePipe, LoadingComponent],
  templateUrl: './orders-table.component.html',
})
export class OrdersTableComponent {
  // input
  isLoading = input.required<boolean>();
  currOrders = input.required<IHistoryOrderDTO[]>();
  currentSection = input.required<number>();
  numberOfSections = input.required<number>();
  allOrdersCount = input.required<number>();

  // output
  invokeAction = output<string>();
  changeSectionOutput = output<number>();


  // computed
  BottomNumbers = computed<number[]>((maxLen: number = 3) => {
    const numberOfSections = this.numberOfSections();
    const currentSection = this.currentSection();

    let canTakeRight = numberOfSections - currentSection;
    let canTakeLeft = currentSection - 1;

    const isEven = maxLen % 2 == 0 ? 1 : 0;
    const maxCanTake = Math.trunc(maxLen / 2) - isEven;

    let takeRight = Math.min(maxCanTake + isEven, canTakeRight);
    canTakeRight -= takeRight;

    let takeLeft = Math.min(maxCanTake, canTakeLeft);
    canTakeLeft -= takeLeft;

    if (takeLeft < maxCanTake) takeRight += Math.min(maxCanTake - takeLeft, canTakeRight);
    else if (takeRight < maxCanTake + isEven)
      takeLeft += Math.min(maxCanTake + isEven - takeRight, canTakeLeft);

    let begin = currentSection - takeLeft;
    const finalLen = 1 + takeRight + takeLeft;
    const resArr = Array.from({ length: finalLen }, () => begin++);

    return resArr;
  });


  // methods
  
  // get status name
  getStatusName = (status: OrderStateEnum) => {
    return OrderStateEnum[status];
  };

  // change section
  changeSection = (newSection: number) => {
    if (this.currentSection() === newSection) return;
    if (newSection < 1 || newSection > this.numberOfSections()) return;

    this.changeSectionOutput.emit(newSection);
  }
}
