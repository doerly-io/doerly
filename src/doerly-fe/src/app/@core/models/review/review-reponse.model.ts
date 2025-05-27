export interface ReviewResponse {
  id: number;
  rating: number;
  comment: string;
  createdAt: Date;
  reviewerProfile: ReviewerProfileResponse;
}

export interface ReviewerProfileResponse {
  profileId: number;
  fullName: string;
  avatarUrl: string;
}
