import { OrderStateEnum } from "../../../../../core/enums/order-status-enum";

export interface IOrderDTO {
  id: string;
  title: string;
  image: string;
  status: OrderStateEnum;
  deliveryDate: string;
}
