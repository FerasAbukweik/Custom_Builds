import { IOrderDTO } from "./mini-order-dto";

export interface IHistoryOrderDTO extends IOrderDTO {
    totalPrice: number;
    specs: string[];
    quantity: number;
}