import { Component, computed, inject, signal } from '@angular/core';
import { AdminService } from '../../../admin-service';
import { StateDataComponent } from '../../../../../shared/components/stateData/state-data.component';
import { stateCardData } from '../../../../../core/interfaces/state-card-data';

@Component({
  selector: 'app-dashboard.component',
  imports: [StateDataComponent],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent {
  // DI
  protected readonly adminService = inject(AdminService);

  // signals
  selectedRevenueFilter = signal<'7Days' | 'Month'>('7Days');

  // computed
  linePath = computed(() => {
    let data: number[];

    if (this.selectedRevenueFilter() === '7Days')
      data = this.adminService.dashboardData().weeklyRevenue;
    else if (this.selectedRevenueFilter() === 'Month')
      data = this.adminService.dashboardData().monthlyRevenue;
    else return;

    return this.computeLinePath(data);
  });
  stateCardData = computed(() => {
    const data: stateCardData = [];

    data.push({
      name: 'Total Revenue',
      value: this.adminService.dashboardData().totalRevenue.toFixed(2) + '$',
    });

    data.push({
      name: "Pending Orders",
      value: this.adminService.dashboardData().pendingOrdersCount.toString(),
    })

    data.push({
      name: 'Low Stock',
      value: this.adminService.dashboardData().lowStockAlerts.toString()
    })

    return data;
  });

  // methods
  ngOnInit(): void {
    this.adminService.updateDashboardData();
  }

  // private

  private computeLinePath(data: number[]) {
    if (!data || data.length === 0) return;

    const svgWidth = 800;
    const svgHeight = 300;
    const paddingBottom = 40;
    const chartHeight = svgHeight - paddingBottom;

    const maxVal = Math.max(...data) || 1;
    const xStep = svgWidth / (data.length - 1);

    const points = data.map((val, i) => {
      const x = i * xStep;
      const y = chartHeight - (val / maxVal) * (chartHeight - 20);
      return { x, y };
    });

    let lineD = `M ${points[0].x} ${points[0].y}`;

    for (let i = 0; i < points.length - 1; i++) {
      const p0 = points[i];
      const p1 = points[i + 1];

      const cpX1 = p0.x + xStep / 3;
      const cpY1 = p0.y;
      const cpX2 = p1.x - xStep / 3;
      const cpY2 = p1.y;

      lineD += ` C ${cpX1} ${cpY1}, ${cpX2} ${cpY2}, ${p1.x} ${p1.y}`;
    }

    return lineD;
  }

  isLowStock(inStock: number) {
    return inStock < 10;
  }
}
