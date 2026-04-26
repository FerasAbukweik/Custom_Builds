import { Component, inject, signal } from '@angular/core';
import { LogoComponent } from '../../shared/components/logo.component/logo.component';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { IFooterExtraPages, IFormGroupType, PageTypes } from './login-signup.model';
import { customValidation } from './login-signup.validations';
import { LoginSignupService } from './login-signup.service';
import { customError } from './login-signup.constants';

const footerExtraPages: IFooterExtraPages[] = [
  { icon: 'fa-google', color: '#34A850', title: 'Google', link: '' },
  {
    icon: 'fa-github',
    color: 'white',
    title: 'My GitHub',
    link: 'https://github.com/FerasAbukweik',
  },
  { icon: 'fa-facebook-f', color: '#1877F2', title: 'Facebook', link: '' },
];

@Component({
  selector: 'app-login-signup',
  imports: [LogoComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './login-signup.component.html',
})
export class LoginSignupComponent {
  loginSignupService = inject(LoginSignupService);

  footerExtraPages = footerExtraPages;

  get currPage() {
    return this.loginSignupService.currPage();
  }
  isShowPassword = signal(false);

  form: FormGroup<IFormGroupType> = new FormGroup<IFormGroupType>({
    email: new FormControl('', [customValidation.email()]),
    password: new FormControl('', [customValidation.password()]),
    phoneNumber: new FormControl('', [customValidation.phoneNumber({ skipOn: 'signin' })]),
    userName: new FormControl('', [customValidation.userName({ skipOn: 'signin' })]),
  });

  isError(checkFor: keyof IFormGroupType) {
    const control = this.form.controls[checkFor];
    return control.hasError(customError) && control.touched;
  }

  get allErrors() {
    const errors: string[] = [];

    for (const [key, control] of Object.entries(this.form.controls)) {
      if (this.isError(key as keyof IFormGroupType)) {
        errors.push(control.errors![customError]);
      }
    }

    return errors;
  }

  changePage(newPage: PageTypes) {
    if (this.currPage === newPage) return;

    this.loginSignupService.changePage(newPage);
    this.form.reset();
  }

  onSubmit() {
    this.form.markAllAsTouched();

    if (this.allErrors.length > 0) return;

    if (this.currPage === 'signin') {
      // ------------------------ Signin ------------------------
    } else if (this.currPage === 'createAccount') {
      // ------------------------ Create Account ------------------------
    }
  }
}
