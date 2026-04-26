import { Component, inject } from '@angular/core';
import { ILeftNavItem, IRightNavItem } from '../../../../../interfaces/top-nav/top-nav.model';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { UserPageService } from '../../../../user-page.service';

@Component({
  selector: 'nav[userTopNav]',
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './user-top-nav.component.html',
  host: {
    class:
      '@container absolute top-0 left-0 bg-primary/50 backdrop-blur-lg  w-full border-b-2 border-slate-800 py-2.5 pl-7 pr-2 z-50 transition-all duration-300 flex items-center gap-4',
    '[class.pl-19]': '!userPageService.isSideBarShown()',
  },
})
export class UserTopNavComponent {
  public userPageService = inject(UserPageService);

  leftNavItems: ILeftNavItem[] = [
    { text: 'My Orders', goTo: 'my-orders' },
    { text: 'Saved Builds', goTo: 'saved-builds' },
    { text: 'History', goTo: 'history' },
    { text: 'Support', goTo: 'support' },
  ];

  rightNavItems: IRightNavItem[] = [
    { icon: 'fa-solid fa-cart-shopping', goTo: '/cart' },
    { icon: 'fa-regular fa-user', goTo: '/user' },
  ];
}
