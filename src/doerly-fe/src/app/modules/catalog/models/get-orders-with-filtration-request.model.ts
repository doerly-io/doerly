export interface GetOrdersWithFiltrationRequest {
    pageInfo: {
        number: number;
        size: number;
    };
    categoryId: number;
    isOrderByPrice: boolean;
    isDescending: boolean;
    minPrice?: number | null;
    maxPrice?: number | null;
    searchValue?: string | null;
    regionId?: number | null;
}