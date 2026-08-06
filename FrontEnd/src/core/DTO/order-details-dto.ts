import { OrderStateEnum } from '../enums/order-status-enum';

export interface OrderDetailsDto {
  id: string;
  orderedDate: Date;
  userName: string;
  phoneNumber: string;
  userId: string;
  status: OrderStateEnum;
}
