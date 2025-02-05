export interface BaseApiResponse<TValue = unknown> {
  isSuccess: boolean;
  errorMessage: string | null;
  value: TValue | null;
}
