import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserSideBarComponent } from './components/user-side-bar.component/user-side-bar.component';
import { UserPageService } from '../../user-page.service';
import { UserTopNavComponent } from './components/user-top-nav.component/user-top-nav.component';

@Component({
  selector: 'app-user-layout',
  imports: [RouterOutlet, UserSideBarComponent, UserTopNavComponent],
  templateUrl: './user.layout.html',
  host: {
    class:
      "grid relative w-full h-screen overflow-hidden grid-cols-[auto_1fr] grid-rows-[auto_1fr] [grid-template-areas:'sideBar_TopNav'_'sideBar_content']",
  },
})
export class UserLayout {
  userPageService = inject(UserPageService);

  get isSideBarShown() {
    return this.userPageService.isSideBarShown();
  }
}
