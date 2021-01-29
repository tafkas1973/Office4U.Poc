import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

import { RolesModalComponent } from '../roles-modal/roles-modal.component';
import { User } from '../../_models/user';
import { AdminService } from '../../_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit, OnDestroy {
  notifier = new Subject();
  users: Partial<Array<User>>;
  bsModalRef: BsModalRef;

  constructor(
    private adminService: AdminService,
    private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles()
      .pipe(takeUntil(this.notifier))
      .subscribe(users => this.users = users); // TODO: unsubscribe !!
  }

  openRolesModal(user: User) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roles: this.getRolesArray(user)
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles
      .pipe(takeUntil(this.notifier))    
      .subscribe(values => {
        const rolesToUpdate = {
          roles: [...values.filter(el => el.checked === true).map(el => el.name)]
        };
        if (rolesToUpdate) {
          this.adminService.updateUserRoles(user.username, rolesToUpdate.roles)
            .pipe(takeUntil(this.notifier))
            .subscribe(() => {
              user.roles = [...rolesToUpdate.roles]
            })
        }
      })
  }

  private getRolesArray(user: User) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: Array<any> = [
      { name: 'Admin', value: 'Admin' },
      { name: 'User', value: 'User' },
      { name: 'ManageArticles', value: 'ManageArticles' },
      { name: 'ImportArticles', value: 'ImportArticles' },
      { name: 'ExportArticles', value: 'ExportArticles' }
    ];

    availableRoles.forEach(role => {
      let isMatch = false;
      for (const userRole of userRoles) {
        if (role.name === userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;
  }

  ngOnDestroy() {
    this.notifier.next();
    this.notifier.complete();
  }  
}
