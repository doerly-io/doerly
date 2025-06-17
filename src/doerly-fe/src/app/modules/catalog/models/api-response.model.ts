export interface ApiResponse<T> {
  value: T;
  isSuccess: boolean;
  errorMessage: string;
} 