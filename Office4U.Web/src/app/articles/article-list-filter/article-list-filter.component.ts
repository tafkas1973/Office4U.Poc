import { Component, Input, OnInit, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

import { ArticleParams } from '../../_models/articleParams';

@Component({
  selector: 'app-article-list-filter',
  templateUrl: './article-list-filter.component.html',
  styleUrls: ['./article-list-filter.component.css']
})
export class ArticleListFilterComponent implements OnInit {
  @Input() articleParams: ArticleParams;
  @Output() loadArticlesEvent = new EventEmitter();
  @Output() resetFiltersEvent = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  loadArticles() {
    this.loadArticlesEvent.emit(null);
  }

  resetFilters() {
    this.resetFiltersEvent.emit(null);
  }
}
