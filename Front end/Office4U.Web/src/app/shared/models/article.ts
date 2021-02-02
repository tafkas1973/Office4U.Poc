import { ArticlePhoto } from "./articlePhoto";

export interface Article {
    id: number;
    code: string;
    supplierId: string;
    supplierReference: string;
    name1: string;
    unit: string;
    purchasePrice: number;
    photoUrl: string;
    photos: Array<ArticlePhoto>;
}

export interface ArticleForCreation {
    code: string;
    supplierId: string;
    supplierReference: string;
    name1: string;
    unit: string;
    purchasePrice: number;
  }
