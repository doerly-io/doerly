export enum EFilterType {
    Checkbox = 1,
    Dropdown = 2,
    Price = 3,
    Radio = 4
}

export interface IFilter {
    id: number;
    name: string;
    type: EFilterType;
    options: string[] | null;
    categoryId: number;
}

export interface IFilterValue {
    filterId: number;
    value: string;
}

export interface IFilterResponse {
    value: IFilter[];
    isSuccess: boolean;
    errorMessage: string;
} 