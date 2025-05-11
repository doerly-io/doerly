import {MessageTypeEnum} from './message.type.enum';
import {MessageStatus} from './message.status.enum';


export interface MessageResponse {
  id: number;
  conversationId: number;
  // conversation: ConversationDto;
  messageType: MessageTypeEnum;
  senderId: number;
  messageContent: string;
  sentAt: Date; // ISO string format
  status: MessageStatus;
}

