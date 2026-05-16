import { Component, DestroyRef, inject, signal } from '@angular/core';
import { LogoComponent } from '../../layouts/logo.component/logo.component';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { IFooterExtraPages, IFormGroupType, PageTypes } from './login-signup.model';
import { customValidation } from './login-signup.validations';
import { LoginSignupService } from './login-signup.service';
import { IRegisterDTO } from '../../core/DTO/register-dto';
import { RoleEnums } from '../../core/enums/role-enums';
import { AccountServices } from '../../core/services/account-services';
import { Router } from '@angular/router';
import { ILoginDTO } from '../../core/DTO/login-dto';

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
  // inject services
  private readonly loginSignupService = inject(LoginSignupService);
  private readonly accountServices = inject(AccountServices);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  // so we can access it in the html
  readonly footerExtraPages = footerExtraPages;

  currPage = this.loginSignupService.getCurrPage;
  isShowPassword = signal(false);

  // define form for both login and signup pages
  form: FormGroup<IFormGroupType> = new FormGroup<IFormGroupType>({
    email: new FormControl('', {
      nonNullable: true,
      validators: [customValidation.email()],
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [customValidation.password()],
    }),
    phoneNumber: new FormControl('', {
      nonNullable: true,
      // skip validation on signin page
      validators: [customValidation.phoneNumber({ skipOn: 'signin' })],
    }),
    userName: new FormControl('', {
      nonNullable: true,
      // skip validation on signin page
      validators: [customValidation.userName({ skipOn: 'signin' })],
    }),
  });

  // check if the control has error and touched
  isError(checkFor: keyof IFormGroupType): boolean {
    const control = this.form.controls[checkFor];
    return control.invalid && control.touched && control.dirty;
  }

  // get all errors
  getAllErrors(): string[] {
    let errors: string[] = [];

    // check all controls
    for (const [key, control] of Object.entries(this.form.controls)) {
      // if no error skip
      if (!this.isError(key as keyof IFormGroupType)) continue;

      // get new errors
      const newErrors = Object.keys(control.errors ?? {});

      // add new errors to the errors array
      errors = [...errors, ...newErrors];
    }

    // add server error if exists
    if (this.form.errors?.['server']) {
      errors = [...errors, this.form.errors['server']];
    }

    return errors;
  }

  changePage(newPage: PageTypes) {
    // if same page dont do anything
    if (this.currPage() === newPage) return;

    this.loginSignupService.changePage(newPage);
    this.form.reset();
  }

  onSubmit() {
    // for isError to work Correctly
    this.form.markAllAsTouched();
    this.form.markAllAsDirty();

    // if have any error generated from the customValidation | doesnt check dirty nor touched
    if (!this.form.valid) return;

    // login
    if (this.currPage() === 'signin') {
      const loginData: ILoginDTO = {
        Email: this.form.value.email!,
        Password: this.form.value.password!,
      };

      const sub = this.accountServices.login(loginData).subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.form.setErrors({ server: err?.error ?? 'Unknown server error' });
        },
      });

      this.destroyRef.onDestroy(() => sub.unsubscribe());
    }

    // signup
    else {
      const signupData: IRegisterDTO = {
        Email: this.form.value.email!,
        Password: this.form.value.password!,
        PhoneNumber: this.form.value.phoneNumber!,
        UserName: this.form.value.userName!,
        Role: RoleEnums.User,
      };

      const sub = this.accountServices.register(signupData).subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.form.setErrors({ server: err?.error ?? 'Unknown server error' });
        },
      });

      this.destroyRef.onDestroy(() => sub.unsubscribe());
    }
  }
}
