import { OrderTypeEnum } from "../enums/order-type-enum";

export interface ICartItemDTO {
    id: string;
    orderType: OrderTypeEnum;
    customBuildId: string;
    productId: string;
    totalPrice: number;
    quantity: number;
    title: string;
    specs: string[];
    image: string;
}
