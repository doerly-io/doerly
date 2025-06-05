export interface IEditableCategory {
  name: string;
  description: string;
  parentId: number | null;
  isEnabled: boolean;
}
