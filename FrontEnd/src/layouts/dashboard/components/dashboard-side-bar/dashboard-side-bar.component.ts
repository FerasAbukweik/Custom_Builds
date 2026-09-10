import { Component, inject, input } from '@angular/core';
import { projectName } from '../../../../core/constants/project';
import { ISideBarData } from '../../dashboard.model';
import { RouterModule } from '@angular/router';
import { DashboardService } from '../../dashboard.service';
import { AuthService } from 'src/core/services/client-services/auth-service';

@Component({
  selector: 'aside[dashboardSideBar]',
  imports: [RouterModule],
  templateUrl: './dashboard-side-bar.component.html',
  host: {
    class: 'h-full flex flex-col border-r-2 border-slate-800 p-6.5 pt-0 whitespace-nowrap',
  },
})
export class DashboardSideBarComponent {
  // services
  private _dashboardService = inject(DashboardService);
  protected authService = inject(AuthService);

  // input
  pagesData = input.required<ISideBarData[]>();

  // fields

  // public
  projectName = projectName;

  // methods
  onClick() {
    if (window.innerWidth <= 1024) {
      this._dashboardService.toggleShowSideBar();
    }
  }
}
