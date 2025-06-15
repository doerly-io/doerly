export interface CreateFeedbackRequest {
  rating: number;
  comment?: string;
  executorId: number;
  categoryId: number;
}
