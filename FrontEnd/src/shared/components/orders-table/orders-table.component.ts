import { Component, computed, inject, input, output, signal } from '@angular/core';
import { IOrderDto } from '../../../core/DTO/orders-dto';
import { OrderStateEnum } from '../../../core/enums/order-status-enum';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { LoadingComponent } from '../loading/loading.component';
import { OrderDetailsDialogService } from '../order-details-dialog/order-detaild-dialog-service';

@Component({
  selector: 'app-orders-table',
  imports: [CurrencyPipe, LoadingComponent, DatePipe],
  templateUrl: './orders-table.component.html',
})
export class OrdersTableComponent {
  // DI
  protected readonly orderDetailsDialogService = inject(OrderDetailsDialogService);

  // input
  isLoading = input.required<boolean>();
  sectionsOrders = input.required<IOrderDto[][]>();
  allOrdersCount = input.required<number>();

  // output
  changeSectionOutput = output<number>();

  // signals
  protected currentSection = signal<number>(1);

  // computed
  protected BottomNumbers = computed<number[]>(() => {
    const numberOfSections = this.numberOfSections();
    const currentSection = this.currentSection();
    const maxLen = 3;

    return this.generateBottomNumbers(numberOfSections, currentSection, maxLen);
  });
  protected currOrders = computed<IOrderDto[]>(() => {
    return this.sectionsOrders()[this.currentSection() - 1];
  });
  protected numberOfSections = computed(() => this.sectionsOrders().length);

  // methods

  // get status name
  getStatusName = (state: OrderStateEnum) => {
    return OrderStateEnum[state];
  };

  // change section
  changeSection = (newSection: number) => {
    if (this.currentSection() === newSection) return;
    if (newSection < 1 || newSection > this.numberOfSections()) return;

    this.currentSection.set(newSection);
    this.changeSectionOutput.emit(newSection);
  };

  // private

  private generateBottomNumbers(numberOfSections: number, currentSection: number, maxLen: number) {
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
  }
}
