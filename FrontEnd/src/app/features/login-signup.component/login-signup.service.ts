import { Injectable, signal } from '@angular/core';
import { PageTypes } from './login-signup.model';

@Injectable()
export class LoginSignupService {
  private _currPage = signal<PageTypes>('signin');

  get getCurrPage() {
    return this._currPage.asReadonly();
  }

  changePage(newPage: PageTypes) {
    this._currPage.set(newPage);
  }
}
