import { Component, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { fade } from '../../shared/animations/animation';
import { Article } from '../../shared/models/article';
import { ArticleService } from '../article.service';

@Component({
  selector: 'app-article-edit',
  templateUrl: './article-edit.component.html',
  styleUrls: ['./article-edit.component.css'],
  animations: [fade]
})
export class ArticleEditComponent implements OnInit, OnDestroy {
  @ViewChild('editForm') editForm: NgForm;
  notifier = new Subject();
  article: Article;
  pageTitle = "Article Edit";
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private articleService: ArticleService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.article = data['article'];
    });
  }

  updateArticle() {
    this.articleService
      .updateArticle(this.article)
      .pipe(takeUntil(this.notifier))
      .subscribe(() => {
        this.toastr.success('Article updated succesfully');
        this.editForm.reset(this.article);
        const navigationExtras: NavigationExtras = { state: { forceLoad: true } };
        this.router.navigateByUrl('/article', navigationExtras);
      });
  }

  ngOnDestroy() {
    this.notifier.next();
    this.notifier.complete();
  }
}
