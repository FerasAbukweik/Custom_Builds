import { Injectable, signal } from '@angular/core';

@Injectable()
export class DashboardService {
  private _isSideBarShown = signal<boolean>(window.innerWidth > 650);

  isSideBarShown = this._isSideBarShown.asReadonly();

  toggleShowSideBar() {
    this._isSideBarShown.update((curr) => !curr);
  }
}
