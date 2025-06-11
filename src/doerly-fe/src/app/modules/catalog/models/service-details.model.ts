export interface IServiceDetails {
  id: number;
  name: string;
  description: string;
  categoryId: number;
  categoryName: string;
  userId: number;
  user: {
    id: number;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    sex: number;
    bio: string;
    dateCreated: string;
    lastModifiedDate: string;
    imageUrl: string | null;
    cvUrl: string | null;
    address: {
      cityId: number;
      cityName: string;
      regionId: number;
      regionName: string;
    };
    languageProficiencies: any[];
    competences: {
      id: number;
      categoryId: number;
      categoryName: string;
    }[];
    userInfo: any;
  };
  price: number;
  categoryPath: any[];
  isEnabled: boolean;
  isDeleted: boolean;
  filterValues: {
    filterId: number;
    filterName: string;
    filterType: number;
    value: string;
  }[];
} 