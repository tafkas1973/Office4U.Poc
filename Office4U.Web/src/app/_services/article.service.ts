import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { Article, ArticleForCreation } from '../_models/article';
import { ArticleParams } from '../_models/articleParams';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  baseUrl = environment.apiUrl;
  articles: Array<Article> = [];
  paginatedResult: PaginatedResult<Array<Article>> = new PaginatedResult<Array<Article>>();
  articleParams: ArticleParams;
  articleCache = new Map();

  constructor(
    private http: HttpClient) {
    this.articleParams = new ArticleParams();
  }

  getArticleParams() {
    return this.articleParams;
  }

  setArticleParams(params: ArticleParams) {
    this.articleParams = params;
  }

  resetArticleParams(): ArticleParams {
    this.articleParams = new ArticleParams();
    return this.articleParams;
  }

  getArticles(articleParams: ArticleParams, forceLoad = false) {
    var key = Object.values(articleParams).join('-');
    var response = this.articleCache.get(key);
    if (response && !forceLoad) return of(response);

    if (forceLoad) this.articleCache.clear();

    let params = this.GetPaginationHeaders(articleParams);
    params = this.AppendFilterParams(params, articleParams);
    params = this.AppendOrderByParam(params, articleParams);

    return this.getPaginatedResult<Array<Article>>(this.baseUrl + 'articles', params)
      .pipe(map(response => {
        this.articleCache.set(key, response);
        return response;
      }));
  }

  getArticle(id: number) {
    const article = [...this.articleCache.values()]
      .reduce((prevArr, currElem) => prevArr.concat(currElem.result), [])
      .find((article: Article) => article.id === id);
    if (article) return of(article);

    return this.http.get<Article>(this.baseUrl + 'articles/' + id.toString());
  }

  createArticle(article: ArticleForCreation): Observable<Object> {
    return this.http.post(this.baseUrl + 'articles', article);
  }

  updateArticle(article: Article) {
    return this.http
      .put(this.baseUrl + 'articles', article)
      .pipe(
        map(() => {
          const index = this.articles.indexOf(article);
          this.articles[index] = article;
        })
      );
  }

  deleteArticle(id: number): Observable<Object> {
    return this.http.delete(this.baseUrl + 'articles/' + id);
  }

  private GetPaginationHeaders(articleParams: ArticleParams) {
    let params = new HttpParams();

    params = params.append('pageNumber', articleParams.pageNumber.toString());
    params = params.append('pageSize', articleParams.pageSize.toString());
    return params;
  }

  private AppendFilterParams(params: HttpParams, articleParams: ArticleParams) {
    if (articleParams.code !== undefined) {
      params = params.append('code', articleParams.code);
    }
    if (articleParams.supplierId !== undefined) {
      params = params.append('supplierId', articleParams.supplierId);
    }
    if (articleParams.supplierReference !== undefined) {
      params = params.append('supplierReference', articleParams.supplierReference);
    }
    if (articleParams.name1 !== undefined) {
      params = params.append('name1', articleParams.name1);
    }
    if (articleParams.unit !== undefined) {
      params = params.append('unit', articleParams.unit);
    }
    if (articleParams.purchasePriceMin !== undefined) {
      params = params.append('purchasePriceMin', articleParams.purchasePriceMin.toString());
    }
    if (articleParams.purchasePriceMax !== undefined) {
      params = params.append('purchasePriceMax', articleParams.purchasePriceMax.toString());
    }

    return params;
  }

  private AppendOrderByParam(params: HttpParams, articleParams: ArticleParams) {
    params = params.append('orderBy', articleParams.orderBy);

    return params;
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    // we also want the response body back with pagination info (observe response)
    return this.http.get<T>(url, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }
}
