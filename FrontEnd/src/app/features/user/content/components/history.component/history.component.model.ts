export interface IStatsData {
  label: string;
  value: string;
  highlight?: boolean;
  badge?: string;
}

export interface IOrderHistory {
  id: string;
  date: string;
  item: string;
  specs: string;
  status: string;
  price: string;
  statusType: 'ok' | 'error';
}