export interface IStep {
  id: number;
  label: string;
  date: string;
  status: 'completed' | 'current' | 'upcoming';
  icon: string;
}