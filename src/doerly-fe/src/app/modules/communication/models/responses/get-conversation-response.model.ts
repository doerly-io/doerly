import {ConversationHeaderResponse} from '../conversation-header-response.model';

export interface GetConversationResponse {
  total: number;
  conversations: ConversationHeaderResponse[];
}
