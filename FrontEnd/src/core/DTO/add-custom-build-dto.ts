import { CustomBuildTypeEnum } from '../enums/custom-build-type-enum';

export interface ICustomBuildAddDTO {
  modificationIds: string[];
  customBuildType: CustomBuildTypeEnum;
}
