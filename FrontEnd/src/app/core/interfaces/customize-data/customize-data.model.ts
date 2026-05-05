interface IModification {
  id: number;
  name: string;
  value?: string;
  desc?: string;
  icon?: string;
  price: Number;
  type: string;
}

interface IField {
  id: number;
  title: string;
  items: IModification[];
}

export interface ISection {
  id: number;
  icon: string;
  fields: IField[];
}
