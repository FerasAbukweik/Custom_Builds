import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { projectName } from '../../../../../../core/constants/constants';
import { ISideBarData } from './user-side-bar.model';
import { RouterModule } from '@angular/router';
import { UserPageService } from '../../../../user-page.service';

@Component({
  selector: 'aside[userSideBar]',
  imports: [RouterModule],
  templateUrl: './user-side-bar.component.html',
  host: {
    class: 'h-full border-r-2 border-slate-800 p-6.5 whitespace-nowrap',
  },
})
export class UserSideBarComponent {
  projectName = projectName;
  private userPageService = inject(UserPageService);

  pagesData: ISideBarData[] = [
    { id: 1, icon: 'fa-solid fa-box-open', name: 'My Orders', goTo: 'my-orders' },
    { id: 2, icon: 'fa-regular fa-heart', name: 'Saved Builds', goTo: 'saved-builds' },
    { id: 3, icon: 'fa-solid fa-clock-rotate-left', name: 'History', goTo: 'history' },
    { id: 4, icon: 'fa-solid fa-headset', name: 'Support', goTo: 'support' },
  ];

  onClick() {
    if(window.innerWidth <= 650){
      this.userPageService.toggleShowSideBar();
    }
  }
}
