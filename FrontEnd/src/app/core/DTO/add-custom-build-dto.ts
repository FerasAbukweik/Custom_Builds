import { CustomBuildTypeEnum } from '../enums/custom-build-type-enum';

export interface IAddCustomBuildDTO {
  modificationIds: string[];
  customBuildType: CustomBuildTypeEnum;
}
