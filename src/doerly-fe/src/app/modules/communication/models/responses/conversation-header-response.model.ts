import {ProfileResponse} from '../../../profile/models/responses/ProfileResponse';
import {MessageResponse} from './message-response.model';

export interface ConversationHeaderResponse {
  id: number;
  initiator: ProfileResponse;
  recipient: ProfileResponse;
  lastMessage: MessageResponse;
}
