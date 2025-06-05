export interface IGetCategoryResponse {
  id: number;
  name: string;
  description?: string;
  isDeleted: boolean;
  isEnabled: boolean;
  children: IGetCategoryResponse[];
}
