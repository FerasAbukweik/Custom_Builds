export interface IMessage {
  id: number;
  sender: 'agent' | 'user';
  name: string;
  role?: string;
  content: string;
  time: string;
  type: 'text' | 'file';
  fileName?: string;
}
