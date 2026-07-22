import { Component, inject, input } from '@angular/core';
import { IRightNavItem } from '../../../../../core/interfaces/top-nav.model';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { DashboardService } from '../../dashboard.service';
import { ISideBarData } from '../../dashboard.model';

@Component({
  selector: 'nav[userTopNav]',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './dashboard-top-nav.component.html',
  host: {
    class:
      '@container sticky top-0 left-0 bg-primary/50 backdrop-blur-lg  w-full border-b-2 border-slate-800 py-2.5 pl-7 pr-2 z-50 transition-all duration-300 flex items-center gap-4',
    '[class.pl-19]': '!dashboardService.isSideBarShown()',
  },
})
export class DashboardTopNavComponent {
  // injections
  public dashboardService = inject(DashboardService);

  // input
  leftNavItems = input.required<ISideBarData[]>();

  rightNavItems: IRightNavItem[] = [
    { icon: 'fa-solid fa-cart-shopping', goTo: '/cart' },
    { icon: 'fa-regular fa-user', goTo: '/user' },
  ];
}
