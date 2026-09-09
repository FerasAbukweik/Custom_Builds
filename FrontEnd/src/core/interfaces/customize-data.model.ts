export interface IModification {
  id: string;
  name: string;
  image?: string;
  price: number;
}

export interface ISection {
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
