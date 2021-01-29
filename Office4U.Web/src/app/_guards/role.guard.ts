import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private toastr: ToastrService) { }

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.accountService.currentUser$
      .pipe(map(user => {
        let role = route.data.role as string;
        if (role && user.roles.includes(role)) {
          return true;
        }
        this.toastr.error('You cannot enter this area');
      }))
  }
}
