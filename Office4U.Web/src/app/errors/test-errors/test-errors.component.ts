import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit, OnDestroy {
  notifier = new Subject();
  baseUrl = environment.apiUrl;
  validationErrors: Array<string> = [];
  pageTitle = "Errors";

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  get404Error() {
    this.http
      .get(this.baseUrl + 'buggy/not-found')
      .pipe(takeUntil(this.notifier))
      .subscribe(response => {
        console.log(response);
      }, error => {
        console.log(error);
      })
  }

  get400Error() {
    this.http
      .get(this.baseUrl + 'buggy/bad-request')
      .pipe(takeUntil(this.notifier))
      .subscribe(response => {
        console.log(response);
      }, error => {
        console.log(error);
      })
  }

  get500Error() {
    this.http
      .get(this.baseUrl + 'buggy/server-error')
      .pipe(takeUntil(this.notifier))
      .subscribe(response => {
        console.log(response);
      }, error => {
        console.log(error);
      })
  }

  get401Error() {
    this.http
      .get(this.baseUrl + 'buggy/auth')
      .pipe(takeUntil(this.notifier))
      .subscribe(response => {
        console.log(response);
      }, error => {
        console.log(error);
      })
  }

  get400ValidationError() {
    this.http
      .post(this.baseUrl + 'account/register', {})
      .pipe(takeUntil(this.notifier))
      .subscribe(response => {
        console.log(response);
      }, error => {
        console.log(error);
        this.validationErrors = error;
      })
  }

  ngOnDestroy() {
    this.notifier.next();
    this.notifier.complete();
  }
}
