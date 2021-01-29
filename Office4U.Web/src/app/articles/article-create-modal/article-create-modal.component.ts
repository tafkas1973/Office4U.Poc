import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ArticleForCreation } from '../../_models/article';

@Component({
  selector: 'app-article-create-modal',
  templateUrl: './article-create-modal.component.html',
  styleUrls: ['./article-create-modal.component.css']
})
export class ArticleCreateModalComponent implements OnInit {
  @Input() validationErrors: Array<string> = [];
  @Output() createArticleEvent: EventEmitter<any> = new EventEmitter();
  articleCreateForm: FormGroup;
  data: any;
  public getServerSideValidation = false; // for demo purposes

  constructor(
    public modalRef: BsModalRef,
    private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.articleCreateForm = this.fb.group({
      code: ['', Validators.required],
      name1: ['', Validators.required],
      supplierId: ['', Validators.required],
      supplierReference: ['', [Validators.required, Validators.maxLength(150)]],
      unit: ['', Validators.required],
      purchasePrice: ['', Validators.required]
    });
  }

  createArticle() {
    if (this.articleCreateForm.valid || this.getServerSideValidation) {
      const newArticle: ArticleForCreation = {
        code: this.articleCreateForm.value.code,
        name1: this.articleCreateForm.value.name1,
        supplierId: this.articleCreateForm.value.supplierId,
        supplierReference: this.articleCreateForm.value.supplierReference,
        unit: this.articleCreateForm.value.unit,
        purchasePrice: +this.articleCreateForm.value.purchasePrice
      };
      this.createArticleEvent.emit(newArticle);
      if (this.articleCreateForm.valid){
        this.modalRef.hide();
      }
    }
  }
}
