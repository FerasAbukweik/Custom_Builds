interface ISectionItem {
  id: number;
  name: string;
  value?: string;
  desc?: string;
  icon?: string;
  price: Number;
}

interface IField {
  id: number;
  title: string;
  type: string;
  items: ISectionItem[];
}

export interface ISection {
  id: number;
  icon: string;
  fields: IField[];
}
