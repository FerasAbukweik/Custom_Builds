import { Component, inject, input, OnInit } from '@angular/core';
import { projectName } from '../../../../core/constants/constants';
import { ISideBarData } from '../../dashboard.model';
import { RouterModule } from '@angular/router';
import { DashboardService } from '../../dashboard.service';

@Component({
  selector: 'aside[dashboardSideBar]',
  imports: [RouterModule],
  templateUrl: './dashboard-side-bar.component.html',
  host: {
    class: 'h-full border-r-2 border-slate-800 p-6.5 pt-4.5 whitespace-nowrap',
  },
})
export class DashboardSideBarComponent {
  // services
  private dashboardService = inject(DashboardService);

  // input
  pagesData = input.required<ISideBarData[]>();

  // fields

  // public
  projectName = projectName;

  // methods
  onClick() {
    if (window.innerWidth <= 650) {
      this.dashboardService.toggleShowSideBar();
    }
  }
}
