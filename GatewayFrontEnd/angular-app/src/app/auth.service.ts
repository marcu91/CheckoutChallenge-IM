
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable()
export class AuthService implements CanActivate {
  baseAuthUrl: string;
  merchantId: string;

  errors: any = [];

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {
    this.baseAuthUrl = "http://localhost:54176/authenticate";

    //TODO : Refactor back-end to associate a user to a merchant entity
    this.merchantId = "B3E7C684-99A2-4253-898B-01515B92F1B1";
  }

  canActivate(): boolean {
    if (!this.isAuthenticated) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }

  get isAuthenticated() {
    return !!localStorage.getItem("token");
  }

  register(credentials: any) {
    this.http
      .post(this.baseAuthUrl + "/register", credentials, { responseType: 'text' })
      .subscribe((data: any) => {
        this.authenticate(data);
      }, errors => {
        this.errors = JSON.parse(errors.error);
        console.log(this.errors);
      });
  }

  login(credentials: any) {
    this.http
      .post(this.baseAuthUrl + "/login", credentials, { responseType: 'text' })
      .subscribe((data: any) => {
        this.authenticate(data);
      }, errors => {
        alert("User name or password are incorrect. Please retry");
        console.log(this.errors);
      });
  }

  logout() {
    localStorage.removeItem("token");
    this.router.navigate(["/login"]);
  }

  private authenticate(data: any) {
    console.log("token:", data);

    var loginData = JSON.parse(data);
    localStorage.setItem("token", loginData.token);
    localStorage.setItem("merchantId", this.merchantId);
    this.router.navigate(["/checkout"]);
  }
}
