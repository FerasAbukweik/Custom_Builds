interface ISectionItem {
  id: number;
  name: string;
  value?: string;
  desc?: string;
  icon?: string;
}

interface ISelection {
  id: number;
  title: string;
  price: string;
  type: string;
  items: ISectionItem[];
}

export interface ICustomizeData {
  id: number;
  icon: string;
  sections: ISelection[];
}
