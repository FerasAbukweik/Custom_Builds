import { MessageTypeEnum } from '../enums/message-type-enum';

export interface IMessageDTO {
  id: number;
  isCurrUserSender: boolean;
  senderName: string;
  role?: string;
  content: string;
  createdAt: string;
  messageType: MessageTypeEnum;
  fileName?: string;
}
