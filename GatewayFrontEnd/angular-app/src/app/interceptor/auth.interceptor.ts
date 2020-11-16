
import { HttpInterceptor } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  baseAuthUrl: string;

  constructor(
  ) {
    this.baseAuthUrl = "http://localhost:54176/authenticate/login";
  }

  intercept(req: any, next: any) {
    console.log(req);
    var token = localStorage.getItem("token");
    var authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    })
    return next.handle(authReq);
  }

}
