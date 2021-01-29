import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { ArticleEditComponent } from '../articles/article-edit/article-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(component: ArticleEditComponent): boolean {
    if (component.editForm.dirty) {
      return confirm('Are you sure to continue? Any unsaved changes will be lost');
    }
    return true;
  }
}
