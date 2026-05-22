import { OrderTypeEnum } from '../enums/order-type-enum';

export interface IMiniCartItemDTO {
  id: string;
  orderType: OrderTypeEnum;
  customBuildId: string;
  productId: string;
  Price: number;
  quantity: number;
  title: string;
  specs: string[];
  image: string;
}