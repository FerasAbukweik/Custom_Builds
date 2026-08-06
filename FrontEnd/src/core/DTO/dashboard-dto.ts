import { MiniInventoryItemDto } from "./mini-inventory-item-dto";

export interface DashboardDto {
  totalRevenue: number;
  pendingOrdersCount: number;
  lowStockAlerts: number;
  weeklyRevenue: number[];
  monthlyRevenue: number[];
  inventoryItems: MiniInventoryItemDto[];
}