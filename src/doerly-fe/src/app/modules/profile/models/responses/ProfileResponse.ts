export interface ProfileAddress {
  cityId: number;
  cityName: string;
  regionId: number;
  regionName: string;
}

export interface ProfileResponse {
  id: number;
  firstName: string;
  lastName: string;
  patronymic: string;
  dateOfBirth: string;
  sex: number;
  bio: string;
  imageUrl: string;
  cvUrl: string;
  address?: ProfileAddress;
  languageProficiencies: any[];
}
