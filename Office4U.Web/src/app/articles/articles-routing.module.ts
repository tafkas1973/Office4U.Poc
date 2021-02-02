import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { ArticlesComponent } from "./articles.component";
import { ArticleListFilterComponent } from "./article-list-filter/article-list-filter.component";
import { ArticleListComponent } from "./article-list/article-list.component";
import { ArticleCreateModalComponent } from "./article-create-modal/article-create-modal.component";
import { ArticleDetailComponent } from "./article-detail/article-detail.component";
import { ArticleEditComponent } from "./article-edit/article-edit.component";
import { PreventUnsavedChangesGuard } from "../_guards/prevent-unsaved-changes.guard";
import { AuthGuard } from "../_guards/auth.guard";
import { RoleGuard } from "../_guards/role.guard";

const routes: Routes = [
  {
    path: '',
    component: ArticlesComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'ManageArticles' },
    children: [
      {
        path: '',
        component: ArticleListComponent,
      },
      {
        path: ':id',
        component: ArticleDetailComponent
      },
      {
        path: ':id/edit',
        component: ArticleEditComponent,
        canDeactivate: [PreventUnsavedChangesGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ArticlesRoutingModule {
  static components = [
    ArticleCreateModalComponent,
    ArticleDetailComponent,
    ArticleEditComponent,
    ArticleListComponent,
    ArticleListFilterComponent,
    ArticlesComponent
  ];
}
