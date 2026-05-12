import { FormControl } from "@angular/forms";

export type PageTypes = 'signin' | 'createAccount';

export interface IFormGroupType {
  email: FormControl<string>;
  password: FormControl<string>;
  phoneNumber: FormControl<string>;
  userName: FormControl<string>;
}

export interface IFooterExtraPages {
  icon: string;
  color: string;
  title: string;
  link: string;
}