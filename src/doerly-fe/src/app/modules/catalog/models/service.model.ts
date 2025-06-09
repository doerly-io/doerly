export interface IFilterValueRequest {
    filterId: number;
    value: string;
}

export interface IService {
    id: number;
    name: string;
    description?: string;
    categoryId: number;
    categoryName?: string;
    userId: number;
    price?: number;
    categoryPath: string[];
    isEnabled: boolean;
    isDeleted: boolean;
    imageUrl?: string;
    filterValues: IFilterValueRequest[];
}

export interface IServiceResponse {
    value: IService[];
    isSuccess: boolean;
    errorMessage: string;
}

export interface ICreateServiceRequest {
    name: string;
    description?: string;
    categoryId: number;
    userId: number;
    price?: number;
    filterValues: IFilterValueRequest[];
}

export interface IUpdateServiceRequest {
    name: string;
    description?: string;
    categoryId: number;
    price?: number;
    isEnabled: boolean;
    filterValues: IFilterValueRequest[];
}

export interface IPaginatedServiceResponse {
    value: {
        total: number;
        orders: IService[];
    };
    isSuccess: boolean;
    errorMessage?: string;
} 