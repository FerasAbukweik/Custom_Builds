export interface ProductEditDTO {
  id: string;
  name?: string | null;
  description?: string | null;
  price?: number | null;
  inStock?: number | null;
}