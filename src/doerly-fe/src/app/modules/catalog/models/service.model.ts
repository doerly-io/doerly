export interface IFilterValueRequest {
    filterId: number;
    value: string;
}

export interface IFilterValueResponse {
    filterId: number;
    value: string;
}

export interface IProfileDto {
    id: number;
    firstName: string;
    lastName: string;
    dateOfBirth?: string;
    sex: number;
    bio?: string;
    dateCreated: string;
    lastModifiedDate: string;
    imageUrl?: string;
    cvUrl?: string;
    address?: {
        cityId: number;
        cityName: string;
        regionId: number;
        regionName: string;
    };
    languageProficiencies: any[];
    competences: any[];
    userInfo?: any;
}

export interface IService {
    id: number;
    name: string;
    description: string;
    categoryId: number;
    categoryName: string;
    userId: number;
    user: IProfileDto | null;
    price: number;
    categoryPath: string[];
    isEnabled: boolean;
    isDeleted: boolean;
    filterValues: IFilterValueResponse[];
    createdDate: string;
}

export interface IServiceResponse {
    value: IService[];
    isSuccess: boolean;
    errorMessage: string;
}

export interface ICreateServiceRequest {
    name: string;
    description: string;
    categoryId: number;
    userId: number;
    price: number;
    isEnabled: boolean;
    filterValues: IFilterValueRequest[];
}

export interface IUpdateServiceRequest {
    id: number;
    name: string;
    description: string;
    categoryId: number;
    price: number;
    isEnabled: boolean;
    filterValues: IFilterValueRequest[];
}

export interface IPaginatedServiceResponse {
    isSuccess: boolean;
    value?: {
        list: IService[];
        totalSize: number;
    };
    error?: string;
} 