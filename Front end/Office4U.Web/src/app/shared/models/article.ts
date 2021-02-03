import { ArticlePhoto } from "./articlePhoto";

export interface Article {
    id: number;
    code: string;
    name1: string;
    supplierId: string;
    supplierReference: string;
    purchasePrice: number;
    unit: string;
    photoUrl: string;
    photos: Array<ArticlePhoto>;
}

export interface ArticleForCreation {
    code: string;
    name1: string;
    supplierId: string;
    supplierReference: string;
    purchasePrice: number;
    unit: string;
  }
