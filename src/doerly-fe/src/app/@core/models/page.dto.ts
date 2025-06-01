export interface PageDto<T> {
  list: T[];
  pageSize: number;
  totalSize: number;
  pagesCount: number;
}
