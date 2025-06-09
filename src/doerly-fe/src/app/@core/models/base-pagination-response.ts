export interface BasePaginationResponse<T> {
    count: number;
    items: T[];
}