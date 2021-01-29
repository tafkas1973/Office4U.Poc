import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './_guards/auth.guard';
import { ArticleDetailComponent } from './articles/article-detail/article-detail.component';
import { ArticleEditComponent } from './articles/article-edit/article-edit.component';
import { ArticleListComponent } from './articles/article-list/article-list.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ExportComponent } from './export/export.component';
import { HomeComponent } from './home/home.component';
import { ImportComponent } from './import/import.component';
import { ManagementComponent } from './management/management.component';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { RoleGuard } from './_guards/role.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'article-list', component: ArticleListComponent,
        canActivate: [RoleGuard],
        data: { role: 'ManageArticles' }
      },
      { path: 'article/:id', component: ArticleDetailComponent },
      {
        path: 'article/edit/:id',
        component: ArticleEditComponent,
        canDeactivate: [PreventUnsavedChangesGuard]
      },
      {
        path: 'import', component: ImportComponent,
        canActivate: [RoleGuard],
        data: { role: 'ImportArticles' }
      },
      {
        path: 'export', component: ExportComponent,
        canActivate: [RoleGuard],
        data: { role: 'ExportArticles' }
      },
      { path: 'management', component: ManagementComponent },
      {
        path: 'admin',
        component: AdminPanelComponent,
        canActivate: [RoleGuard],
        data: { role: 'Admin' }
      }
    ]
  },
  { path: 'errors', component: TestErrorsComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: NotFoundComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
