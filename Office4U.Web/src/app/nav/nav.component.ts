import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
import { StyleManagerService } from '../_services/style-manager.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit, OnDestroy {
  notifier = new Subject();
  model: any = {};
  switchToMode = 'Dark mode';

  constructor(
    public accountService: AccountService,
    private router: Router,
    private styleManagerService: StyleManagerService,
    private toastr: ToastrService) { }

  ngOnInit(): void { }

  login() {
    this.accountService
      .login(this.model)
      .pipe(takeUntil(this.notifier))
      .subscribe(response => {
        this.router.navigateByUrl('/article-list');
        this.toastr.success('Login successful');
      })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  getCurrentUser() {
    this.accountService.currentUser$
      .pipe(takeUntil(this.notifier))
      .subscribe(user => {
      }, error => console.log(error))
  }

  onChangeTheme() {
    if (this.switchToMode == 'Dark mode') {
      this.styleManagerService.setStyle('theme', '../../assets/darkly.bootstrap.min.css');
      this.switchToMode = 'Light mode';
    } else {
      this.styleManagerService.setStyle('theme', '../../assets/lumen.bootstrap.min.css');
      this.switchToMode = 'Dark mode';
    }
  }

  ngOnDestroy() {
    this.notifier.next();
    this.notifier.complete();
  }  
}
