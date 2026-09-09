export interface ProductAddDTO{
    name: string;
    price: number;
    description: string;
    images: File[];
    inStock: number;
}