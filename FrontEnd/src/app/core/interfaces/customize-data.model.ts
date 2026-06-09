interface IModification {
  id: string;
  name: string;
  value?: string;
  desc?: string;
  icon?: string;
  price: number;
  type: string;
}

interface ISection {
  id: string;
  title: string;
  modifications: IModification[];
}

export interface IPart {
  id: string;
  icon: string;
  name: string;
  sections: ISection[];
}
