import { AbstractControl, ValidatorFn } from '@angular/forms';
import { regexPatterns } from '../../core/constants/regex';

const validation = {
  userName: (s: string) => (!s ? 'Username is required' : ''),
  email: (s: string) =>
    !s ? 'Email is required' : regexPatterns.email.test(s) ? '' : 'Wrong email',
  phoneNumber: (s: string) =>
    !s ? 'Phone number is required' : regexPatterns.phoneNumber.test(s) ? '' : 'Wrong Phone number',
  password: (s: string) =>
    !s ? 'Password is required' : regexPatterns.password.test(s) ? '' : 'Password too weak',
};

const customValidationFn = (type: keyof typeof validation): ValidatorFn => {
  return (control: AbstractControl) => {
    const error = validation[type](control.value);

    if (error) {
      return { [error]: true };
    }
    return null;
  };
};

export const customValidation = {
  userName: customValidationFn('userName'),
  email: customValidationFn('email'),
  phoneNumber: customValidationFn('phoneNumber'),
  password: customValidationFn('password'),
};
