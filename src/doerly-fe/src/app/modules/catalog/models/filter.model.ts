export enum FilterType {
    Checkbox = 1,
    Dropdown = 2,
    Price = 3,
    Radio = 4
}

export interface IFilter {
    id: number;
    name: string;
    type: FilterType;
    options?: string[];
    categoryId: number;
}

export interface IFilterResponse {
    value: IFilter[];
    isSuccess: boolean;
    errorMessage?: string;
} 