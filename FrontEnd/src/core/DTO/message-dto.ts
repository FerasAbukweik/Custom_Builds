import { MessageTypeEnum } from '../enums/message-type-enum';

export interface IMessageDTO {
  id: number;
  senderId: string;
  senderName: string;
  content: string;
  createdAt: string;
  chatGroupId: string;
}
