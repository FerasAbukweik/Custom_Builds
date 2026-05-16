import { ILazyLoadingDTO } from "./lazy-loading-dto";

export interface ILazyGetCartItemsDTO extends ILazyLoadingDTO {
  userId?: string;
}