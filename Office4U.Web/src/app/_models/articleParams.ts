import { Article } from './article';

export class ArticleParams {
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'code';

    code: string;
    supplierId: string;
    supplierReference: string;
    name1: string;
    unit: string;
    purchasePriceMin?: number;
    purchasePriceMax?: number;

    constructor() { }
}
