import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './core/errors/not-found/not-found.component';
import { ServerErrorComponent } from './core/errors/server-error/server-error.component';
import { TestErrorsComponent } from './core/errors/test-errors/test-errors.component';
import { ExportComponent } from './export/export.component';
import { HomeComponent } from './home/home.component';
import { ImportComponent } from './import/import.component';
import { ManagementComponent } from './management/management.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { RoleGuard } from './core/guards/role.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'article',
    canActivate: [AuthGuard],
    loadChildren: () => import('./article/article.module')
      .then(m => m.ArticlesModule)
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
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
