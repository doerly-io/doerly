import {MessageTypeEnum} from './enums/message.type.enum';
import {MessageStatus} from './enums/message.status.enum';
import {ProfileResponse} from '../../profile/models/responses/ProfileResponse';


export interface MessageResponse {
  id: number;
  conversationId: number;
  messageType: MessageTypeEnum;
  senderId: number;
  sender?: ProfileResponse;
  messageContent: string;
  sentAt: Date;
  status: MessageStatus;
}

