import {ProfileInfoModel} from 'app/modules/order/models/responses/profile-info-model';

export interface OrderFeedbackResponse {
  feedbackId: number;
  rating: number;
  comment: string;
  userProfile: ProfileInfoModel;
  createdAt: Date;
  updatedAt: Date;
}
