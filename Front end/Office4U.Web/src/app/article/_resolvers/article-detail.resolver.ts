import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Article } from "../../shared/models/article";
import { ArticleService } from "../article.service";

@Injectable({
    providedIn: 'root'
})
export class ArticleDetailResolver implements Resolve<Article> {
    constructor(
        private articleService: ArticleService,
        private router: Router,
        private toastr: ToastrService
    ) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Article> {
        return this.articleService.getArticle(route.params['id']).pipe(
            catchError(error => {
                this.toastr.error('Article detail retrieval failed');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}