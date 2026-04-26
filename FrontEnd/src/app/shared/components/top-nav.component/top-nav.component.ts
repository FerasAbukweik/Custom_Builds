import { Component } from '@angular/core';
import { projectName } from '../../../core/constants/constants';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ILeftNavItem, IRightNavItem } from '../../../features/interfaces/top-nav/top-nav.model';
import { LogoComponent } from '../logo.component/logo.component';

@Component({
  selector: 'nav[topNav]',
  imports: [CommonModule, RouterLink, LogoComponent],
  templateUrl: './top-nav.component.html',
  host: {
    class:
      '@container bg-primary/50 backdrop-blur-lg flex justify-center w-full border-b-2 border-slate-800 sticky top-0 left-0 p-2 z-50',
  },
})
export class TopNavComponent {
  projectName = projectName;

  leftNavItems: ILeftNavItem[] = [
    { text: 'Home', goTo: '/' },
    { text: 'Controllers', goTo: '/controllers' },
    { text: 'Keyboards', goTo: '/keyboards' },
    { text: 'Community', goTo: '/community' },
  ];

  rightNavItems: IRightNavItem[] = [
    { icon: 'fa-solid fa-cart-shopping', goTo: '/cart' },
    { icon: 'fa-regular fa-user', goTo: '/user' },
  ];
}
