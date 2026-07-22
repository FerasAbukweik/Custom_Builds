import { RoleEnums } from "../enums/role-enums";

export interface IRegisterDTO {
  UserName: string;
  Email: string;
  PhoneNumber: string;
  Password: string;
  Role: RoleEnums;
}
