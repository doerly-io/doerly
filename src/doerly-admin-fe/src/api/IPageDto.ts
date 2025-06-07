export interface IPageDto<T> {
  pageSize: number;
  totalSize: number;
  pagesCount: number;
  list: T[];
}
