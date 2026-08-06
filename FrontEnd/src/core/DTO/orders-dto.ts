import { OrderStateEnum } from '../enums/order-status-enum';

export interface IOrderDto {
  id: string;
  createdAt: Date;
  orderStatus: OrderStateEnum;
  orderedPrice: number;
}
