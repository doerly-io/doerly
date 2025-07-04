export enum EFilterType {
    Checkbox = 1,
    Dropdown = 2,
    Price = 3,
    Radio = 4
}

export interface IFilter {
    id: number;
    name: string;
    type: number;
    values: {
        id: string;
        name: string;
        selected?: boolean;
    }[];
    selectedValue?: string;
    min?: number;
    max?: number;
    minValue?: number;
    maxValue?: number;
    range?: [number, number];
    categoryId: number;
}

export interface IFilterResponse {
    value: IFilter[];
    isSuccess: boolean;
    errorMessage?: string;
}

export interface IFilterValueRequest {
    filterId: number;
    value: string;
}

export interface IFilterValueResponse {
    filterId: number;
    value: string;
} 