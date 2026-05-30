import { MessageTypeEnum } from "../enums/message-type-enum";

export interface SendMessageDTO {
  content: string;
  messageType: MessageTypeEnum; 
  fileName?: string; 
}