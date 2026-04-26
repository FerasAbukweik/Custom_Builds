import { FormControl } from "@angular/forms";

export type PageTypes = 'signin' | 'createAccount';

export interface IFormGroupType {
  email: FormControl<string | null>;
  password: FormControl<string | null>;
  phoneNumber: FormControl<string | null>;
  userName: FormControl<string | null>;
}

export interface IFooterExtraPages {
  icon: string;
  color: string;
  title: string;
  link: string;
}