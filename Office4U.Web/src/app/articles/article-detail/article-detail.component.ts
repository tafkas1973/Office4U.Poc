import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { Article } from '../../_models/article';
import { ArticleService } from '../../_services/article.service';
import { fade } from '../../_animations/animations';

@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.css'],
  animations: [fade]
})
export class ArticleDetailComponent implements OnInit, OnDestroy {
  notifier = new Subject();
  article: Article;
  galleryOptions: Array<NgxGalleryOptions>;
  galleryImages: Array<NgxGalleryImage>;
  pageTitle = "Article Detail";

  constructor(
    private articleService: ArticleService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadArticle();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]
  }

  getImages(): Array<NgxGalleryImage> {
    const imageUrls = [];
    for (const photo of this.article.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      });
    }
    return imageUrls;
  }

  loadArticle() {
    this.articleService
      .getArticle((Number)(this.route.snapshot.paramMap.get('id')))
      .pipe(takeUntil(this.notifier))
      .subscribe(article => {
        this.article = article
        this.galleryImages = this.getImages();
      });
  }

  onRemoveArticle() {
    this.removeArticle();
  }

  private removeArticle() {
    var response = confirm('Are you sure you want to delete this article?');
    if (response) {
      this.articleService
        .deleteArticle(this.article.id)
        .pipe(takeUntil(this.notifier))
        .subscribe(
          () => {
            this.toastr.success('Article has been deleted');
            const navigationExtras: NavigationExtras = { state: { forceLoad: true } }; 
            this.router.navigateByUrl('/article-list', navigationExtras);
          },
          error => {
            this.toastr.error('Failed to delete article');
          }
        );
    }
  }

  ngOnDestroy() {
    this.notifier.next();
    this.notifier.complete();
  }
}
