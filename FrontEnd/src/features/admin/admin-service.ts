import { inject, Injectable, signal } from '@angular/core';
import { AdminApiService } from '../../core/services/api-services/admin-api-service';
import { DashboardDto } from '../../core/DTO/dashboard-dto';

@Injectable({ providedIn: 'root' })
export class AdminService {
  // DI
  private readonly _adminApiService = inject(AdminApiService);

  // signals
  private _dashboardData = signal<DashboardDto>({
    inventoryItems: [],
    monthlyRevenue: [],
    weeklyRevenue: [],
    lowStockAlerts: -1,
    pendingOrdersCount: -1,
    totalRevenue: -1,
  });
  private _isUpdating = signal<boolean>(false);

  // getters

  get isUpdating() {
    return this._isUpdating.asReadonly();
  }

  get dashboardData() {
    return this._dashboardData.asReadonly();
  }

  // methods

  updateDashboardData() {
    if (this._isUpdating()) return;
    this._isUpdating.set(true);

    this._adminApiService.getDashboardData().subscribe({
      next: (data) => {
        this._dashboardData.set(data);
        this._isUpdating.set(false);
      },
      error: () => {
        this._isUpdating.set(false);
      },
    });
  }
}
