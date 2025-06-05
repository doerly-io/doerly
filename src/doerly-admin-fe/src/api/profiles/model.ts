import { IPageDto } from 'api/IPageDto';

export interface IProfileInfo {
  id: number;
  firstName: string;
  lastName: string;
  avatarUrl?: string | null;
}

export interface IProfileDto {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth?: string | null; // ISO date string
  sex: ESex;
  bio?: string | null;
  dateCreated: string; // ISO date string
  lastModifiedDate: string; // ISO date string
  imageUrl?: string | null;
  cvUrl?: string | null;
  address?: IProfileAddressDto | null;
  languageProficiencies: ILanguageProficiencyDto[];
  competences: ICompetenceDto[];
}

export interface IProfileSearchResponse extends IPageDto<IProfileDto> {}

export enum ESex {
  None = 0,
  Male = 1,
  Female = 2,
}

export enum ELanguageProficiencyLevel {
  Beginner = 1,
  Elementary = 2,
  Intermediate = 3,
  UpperIntermediate = 4,
  Advanced = 5,
  Proficient = 6,
  Native = 7,
}

export interface ILanguageDto {
  id: number;
  name: string;
  code: string;
}

export interface ILanguageProficiencyDto {
  id: number;
  language: ILanguageDto;
  level: ELanguageProficiencyLevel;
}

export interface ICompetenceDto {
  id: number;
  categoryId: number;
  categoryName: string;
}

export interface IProfileAddressDto {
  cityId: number;
  cityName: string;
  regionId: number;
  regionName: string;
}
