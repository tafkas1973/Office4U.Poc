import { Observable } from 'rxjs';

// use XHRBackend and MockBackend provided by Angular:
// https://blog.thoughtram.io/angular/2016/11/28/testing-services-with-http-in-angular-2.html

export class HttpClientMock {

  constructor() {
    this.get = jest.fn();
    this.post = jest.fn();
    this.put = jest.fn();
    this.patch = jest.fn();
    this.delete = jest.fn();
  }

  get(): Observable<any> {
    throw new Error('Error not implemented');
  }

  post(): Observable<any> {
    throw new Error('Error not implemented');
  }

  put(): Observable<any> {
    throw new Error('Error not implemented');
  }

  patch(): Observable<any> {
    throw new Error('Error not implemented');
  }

  delete(): Observable<any> {
    throw new Error('Error not implemented');
  }
}
