import {ConversationResponse} from '../conversation-response.model';

export interface GetConversationResponse {
  total: number;
  conversations: ConversationResponse[];
}
