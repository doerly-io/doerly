export interface IGetCategoryResponse {
  id: number;
  name: string;
  description?: string;
  isDeleted: boolean;
  isEnabled: boolean;
  children: IGetCategoryResponse[];
}

export interface IGetFilterResponse {
  filters: IFilter[];
}

export interface IFilter {
  id: number;
  name: string;
  type: FilterType;
  options: string[];
  categoryId: number;
}

export enum FilterType {
  Checkbox = 1,
  Dropdown = 2,
  Price = 3,
  Radio = 4,
}
