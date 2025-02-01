export interface BaseApiResponse<TValue = unknown> {
  isSuccess: boolean;
  message: string | null;
  value: TValue | null;
}
