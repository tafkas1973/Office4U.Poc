import { ArticleParams } from "../shared/models/articleParams";

export abstract class ArticleServiceContract {
    abstract baseUrl: string;
    abstract getArticles(articleParams: ArticleParams, forceLoad: boolean);
}