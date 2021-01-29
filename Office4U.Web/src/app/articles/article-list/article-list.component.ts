import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { fade } from '../../_animations/animations';

import { Article, ArticleForCreation } from '../../_models/article';
import { ArticleParams } from '../../_models/articleParams';
import { Pagination } from '../../_models/pagination';
import { ArticleService } from '../../_services/article.service';
import { ArticleCreateModalComponent } from '../article-create-modal/article-create-modal.component';

@Component({
  selector: 'app-articles',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css'],
  animations: [fade]
})
export class ArticleListComponent implements OnInit, OnDestroy {
  public articleParams: ArticleParams;
  isInitialLoad = true;
  isCollapsed = false;
  notifier = new Subject();
  articles: Array<Article>;
  pageTitle = "Articles";
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;
  columnTitles: Array<string> = ['Code', 'Supplier Id', 'Supplier Reference', 'Name', 'Unit', 'Purchase Price(€)'];
  rowCellPropertyNames: Array<string> = ['code', 'supplierId', 'supplierReference', 'name1', 'unit', 'purchasePrice'];
  validationErrors: Array<string> = [];
  modalRef: BsModalRef;
  forceLoad = false;

  constructor(
    private articleService: ArticleService,
    private router: Router,
    private modalService: BsModalService,
    private toastr: ToastrService
  ) {
    this.articleParams = this.articleService.getArticleParams();

    const navigation = this.router.getCurrentNavigation();
    this.forceLoad = navigation?.extras?.state?.forceLoad;
  }

  ngOnInit(): void {
    this.loadArticles(this.forceLoad);
  }

  onPageChanged(event: any, forceLoad = false) {
    this.articleParams.pageNumber = event.page;
    this.articleService.setArticleParams(this.articleParams);
    this.loadArticles(forceLoad);
  }

  onRowClick(articleId: number): void {
    this.router.navigateByUrl("/article/" + articleId.toString());
  }

  onLoadArticles(event: any): void {
    this.loadArticles();
  }

  onResetFilters(event: any): void {
    this.resetFilters();
  }

  onCreateArticle() {
    this.createArticle();
  }

  private loadArticles(forceLoad = false) {
    this.articleService.setArticleParams(this.articleParams);
    this.articleService
      .getArticles(this.articleParams, forceLoad)
      .pipe(takeUntil(this.notifier))
      .subscribe(response => {
        this.articles = response.result;
        this.pagination = response.pagination;
        if (!this.isInitialLoad) {
          this.isCollapsed = true;
        }
        this.isInitialLoad = false;
      })
  }

  private resetFilters() {
    this.articleParams = this.articleService.resetArticleParams();
    this.loadArticles();
  }

  private createArticle() {
    this.validationErrors = [];
    var isCreated = false;
    const config = {
      class: 'modal-dialog-centered',
      ignoreBackdropClick: true,
      initialState: {
        animated: true,
        validationErrors: this.validationErrors,
        isCreated: isCreated
      }
    }
    this.modalRef = this.modalService.show(ArticleCreateModalComponent, config);

    this.modalRef.content.createArticleEvent
      .pipe(takeUntil(this.notifier))
      .subscribe((newArticle: ArticleForCreation) => {
        this.articleService
          .createArticle(newArticle)
          .pipe(takeUntil(this.notifier))
          .subscribe(() => {
            this.loadArticles(true);
            this.toastr.success("Article was created");
          }, error => {
            this.validationErrors = [];
            Object.assign(this.validationErrors, error);
            this.modalRef.content.validationErrors = this.validationErrors;
            this.toastr.error("Failed to create article");
          });
      });

    // we subscribe on the createArticleEvent that is send by the modal before closing
    // we do not want to nest observables, we want an observable chain 
    // use switchMap to create a new observable by taking another observable's data

    // this does only fire once !

    //this.modalRef.content.createArticleEvent
    //  .pipe(
    //    tap(() => console.log('createArticleEvent event fired in modal')),
    //    switchMap((newArticle: ArticleForCreation) => {
    //      return this.articleService.createArticle(newArticle);
    //    })
    //  )
    //  .pipe(takeUntil(this.notifier))
    //  .subscribe(result => {
    //    // refresh activities
    //    this.onPageChanged({ page: 1 }, true);
    //    this.toastr.success("Article was created");
    //  }, error => {
    //    //this.validationErrors = error;
    //    console.log('errors', error);
    //    Object.assign(this.validationErrors, error);
    //    this.toastr.error("Failed to create article");
    //  });
  }

  ngOnDestroy() {
    this.notifier.next();
    this.notifier.complete();
  }
}

