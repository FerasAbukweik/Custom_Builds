import { Component, DestroyRef, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { IFooterExtraPages, IFormGroupType, PageTypes } from './login-signup.model';
import { customValidation } from './login-signup.validations';
import { IRegisterDTO } from '../../core/DTO/register-dto';
import { RoleEnums } from '../../core/enums/role-enums';
import { AccountServices } from '../../core/services/api-services/account-services';
import { Router } from '@angular/router';
import { ILoginDTO } from '../../core/DTO/login-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LogoComponent } from '../../layouts/logo.component/logo.component';

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
  private readonly _accountServices = inject(AccountServices);
  private readonly _router = inject(Router);
  private readonly _destroyRef = inject(DestroyRef);

  // so we can access it in the html
  readonly footerExtraPages = footerExtraPages;

  // signals
  isShowPassword = signal(false);
  currPage = signal<PageTypes>('signin');

  // define form for both login and signup pages
  form: FormGroup<IFormGroupType> = new FormGroup<IFormGroupType>({
    email: new FormControl('', {
      nonNullable: true,
      validators: [customValidation.email],
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [customValidation.password],
    }),
    phoneNumber: new FormControl(
      { value: '', disabled: true },
      {
        nonNullable: true,
        // skip validation on signin page
        validators: [customValidation.phoneNumber],
      },
    ),
    userName: new FormControl(
      { value: '', disabled: true },
      {
        nonNullable: true,
        // skip validation on signin page
        validators: [customValidation.userName],
      },
    ),
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
    if (this.currPage() === newPage) return;

    this.currPage.set(newPage);
    this.form.reset();

    if (newPage === 'signin') {
      this.form.controls.userName.disable();
      this.form.controls.phoneNumber.disable();
    } else {
      this.form.controls.userName.enable();
      this.form.controls.phoneNumber.enable();
    }
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

      this._accountServices
        .login(loginData)
        .pipe(takeUntilDestroyed(this._destroyRef))
        .subscribe({
          next: () => {
            this._router.navigate(['/']);
          },
          error: (err) => {
            console.log(err);
            this.form.setErrors({
              server: (typeof err.error === 'string' && err.error) || 'Unknown server error',
            });
          },
        });
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

      this._accountServices
        .register(signupData)
        .pipe(takeUntilDestroyed(this._destroyRef))
        .subscribe({
          next: () => {
            this._router.navigate(['/']);
          },
          error: (err) => {
            this.form.setErrors({
              server: (typeof err.error === 'string' && err.error) || 'Unknown server error',
            });
          },
        });
    }
  }
}
