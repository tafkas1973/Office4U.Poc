import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable, of } from "rxjs";
import { catchError, mergeMap } from "rxjs/operators";
import { Article } from "../../shared/models/article";
import { PaginatedResult } from "../../shared/models/pagination";
import { ArticleService } from "../article.service";
import { ArticleParams } from "../../shared/models/articleParams";

@Injectable({
    providedIn: 'root'
})
export class ArticleListResolver implements Resolve<PaginatedResult<Array<Article>>> {
    pageNumber = 1;
    pageSize = 5;

    constructor(
        private articleService: ArticleService,
        private router: Router,
        private toast: ToastrService
    ) { }

    resolve(route: ActivatedRouteSnapshot): Observable<PaginatedResult<Array<Article>>> {
        const articleParams: ArticleParams = this.articleService.getArticleParams();
        return this.articleService.getArticles(articleParams)
            .pipe(
                catchError(error => {
                    this.toast.error('Articles retrieval failed');
                    this.router.navigate(['/home']);
                    return of(null);
                }),
                mergeMap(articles => of(articles))
            );
    }
}
