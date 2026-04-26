import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { regexPatterns } from '../../core/constants/regex';
import { PageTypes } from './login-signup.model';
import { inject } from '@angular/core';
import { LoginSignupService } from './login-signup.service';
import { customError } from './login-signup.constants';

const validation = {
  userName: (s: string) => (!s ? 'Username is required' : ''),
  email: (s: string) =>
    !s ? 'Email is required' : regexPatterns.email.test(s) ? '' : 'Wrong email',
  phoneNumber: (s: string) =>
    !s ? 'Phone number is required' : regexPatterns.phoneNumber.test(s) ? '' : 'Wrong Phone number',
  password: (s: string) =>
    !s ? 'Password is required' : regexPatterns.password.test(s) ? '' : 'Password too weak',
  server: (s: string) => '',
};

const customValidationFn = (type: keyof typeof validation, skipOn?: PageTypes): ValidatorFn => {
  const loginSignupService = inject(LoginSignupService);

  return (control: AbstractControl): ValidationErrors | null => {
    if (skipOn) {
      const currPage = loginSignupService.currPage();

      if (currPage === skipOn) return null;
    }

    const error = validation[type](control.value);

    if (error) {
      return { [customError]: error };
    }
    return null;
  };
};

type validationInputType = { skipOn?: PageTypes };

export const customValidation = {
  userName: (input?: validationInputType) => customValidationFn('userName', input?.skipOn),
  email: (input?: validationInputType) => customValidationFn('email', input?.skipOn),
  phoneNumber: (input?: validationInputType) => customValidationFn('phoneNumber', input?.skipOn),
  password: (input?: validationInputType) => customValidationFn('password', input?.skipOn),
  server: (input?: validationInputType) => customValidationFn('server', input?.skipOn),
};
