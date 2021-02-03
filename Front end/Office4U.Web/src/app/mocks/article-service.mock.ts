import { Observable } from "rxjs";
import { ArticleServiceContract } from "../article/article.service.contract";
import { Article } from "../shared/models/article";
import { ArticleParams } from "../shared/models/articleParams";
import { PaginatedResult } from "../shared/models/pagination";

export class ArticleServiceMock implements ArticleServiceContract {

    constructor() {
        this.getArticleParams = jest.fn();
        this.getArticles = jest.fn();
        this.getArticle = jest.fn();
    }

    baseUrl: string;

    getArticleParams(): ArticleParams {
        throw new Error('Method not implemented.');
    }

    getArticles(articleParams: ArticleParams): Observable<PaginatedResult<Array<Article>>> {
        throw new Error('Method not implemented.');
    }

    getArticle(id: number): Observable<Article> {
        throw new Error('Method not implemented.');
    }
}
