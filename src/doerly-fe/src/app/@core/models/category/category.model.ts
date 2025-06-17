export interface ICategory {
  id: number;
  name: string;
  description?: string;
  isDeleted: boolean;
  isEnabled: boolean;
  children: ICategory[];
}
