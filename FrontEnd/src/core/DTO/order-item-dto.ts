import { OrderTypeEnum } from '../enums/order-type-enum';

export interface IOrderItemDto {
  id: string;
  orderType: OrderTypeEnum;
  quantity: number;
  title: string;
  orderedPrice: number;
  image: string;
  state: string;
}
