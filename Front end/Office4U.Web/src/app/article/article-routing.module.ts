import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { ArticleComponent } from "./article.component";
import { ArticleListFilterComponent } from "./article-list-filter/article-list-filter.component";
import { ArticleListComponent } from "./article-list/article-list.component";
import { ArticleCreateModalComponent } from "./article-create-modal/article-create-modal.component";
import { ArticleDetailComponent } from "./article-detail/article-detail.component";
import { ArticleEditComponent } from "./article-edit/article-edit.component";
import { PreventUnsavedChangesGuard } from "../core/guards/prevent-unsaved-changes.guard";
import { AuthGuard } from "../core/guards/auth.guard";
import { RoleGuard } from "../core/guards/role.guard";
import { ArticleListResolver } from "./_resolvers/article-list.resolver";
import { ArticleDetailResolver } from "./_resolvers/article-detail.resolver";

const routes: Routes = [
  {
    path: '',
    component: ArticleComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'ManageArticles' },
    children: [
      {
        path: '',
        component: ArticleListComponent,
        resolve: { articles: ArticleListResolver }
      },
      {
        path: ':id',
        component: ArticleDetailComponent,
        resolve: { article: ArticleDetailResolver }
      },
      {
        path: ':id/edit',
        component: ArticleEditComponent,
        resolve: { article: ArticleDetailResolver },
        canDeactivate: [PreventUnsavedChangesGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ArticleRoutingModule {
  static components = [
    ArticleCreateModalComponent,
    ArticleDetailComponent,
    ArticleEditComponent,
    ArticleListComponent,
    ArticleListFilterComponent,
    ArticleComponent
  ];
}
