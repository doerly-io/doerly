export interface CursorPaginationResponse<TItem> {
  items: TItem[];
  cursor?: string | null;
}
