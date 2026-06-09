import { Component, inject, input } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DashboardSideBarComponent } from './components/dashboard-side-bar.component/dashboard-side-bar.component';
import { DashboardService } from './dashboard.service';
import { DashboardTopNavComponent } from './components/dashboard-top-nav.component/dashboard-top-nav.component';
import { ISideBarData } from './dashboard.model';

@Component({
  selector: 'app-user-layout',
  imports: [RouterOutlet, DashboardSideBarComponent, DashboardTopNavComponent],
  templateUrl: './dashboard.layout.html',
  host: {
    class: "flex flex-row relative w-full h-screen overflow-hidden bg-primary text-white"
  },
})
export class DashboardLayout {
  // injections
  dashboardService = inject(DashboardService);

  // input
  pagesData = input.required<ISideBarData[]>();

  // getters
  get isSideBarShown() {
    return this.dashboardService.isSideBarShown();
  }
}
