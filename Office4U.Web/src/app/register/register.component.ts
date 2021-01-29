import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {
  @Output() cancelRegister = new EventEmitter();
  notifier = new Subject();
  registerForm: FormGroup;
  validationErrors: Array<string> = [];

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: [
        '', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]
      ],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
  }

  register() {
    this.accountService
      .register(this.registerForm.value)
      .pipe(takeUntil(this.notifier))      
      .subscribe(response => {
        this.router.navigateByUrl('/members');
        this.cancel();
      }, error => {
        this.validationErrors = error;
      })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  ngOnDestroy() {
    this.notifier.next();
    this.notifier.complete();
  }
}
