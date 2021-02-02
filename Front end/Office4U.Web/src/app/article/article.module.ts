import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ArticleRoutingModule } from "./article-routing.module";
import { SharedModule } from "../shared/shared.module";
import { O4uInputComponent } from "../shared/o4u-input/o4u-input.component";
import { O4uLabelInputComponent } from "../shared/o4u-label-input/o4u-label-input.component";
import { O4uTableComponent } from "../shared/o4u-table/o4u-table.component";
import { O4uTextareaComponent } from "../shared/o4u-textarea/o4u-textarea.component";

@NgModule({
    declarations: [
        ArticleRoutingModule.components,
        O4uInputComponent,
        O4uLabelInputComponent,
        O4uTableComponent,
        O4uTextareaComponent
    ],
    imports: [
        ArticleRoutingModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        SharedModule
    ],
    exports: [
        CommonModule
    ]
})
export class ArticlesModule { }