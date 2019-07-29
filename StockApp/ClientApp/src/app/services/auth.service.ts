import { Injectable } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";

import { JwtHelperService } from "@auth0/angular-jwt";

import { BehaviorSubject, Observable } from "rxjs";
import { share } from "rxjs/operators";

import { AppSettings } from "./../shared/app.settings";

declare var $: any;
declare var toastmaker: any;

@Injectable({
  providedIn: "root",
})
export class AuthService {
  _jwtHelper: JwtHelperService = new JwtHelperService();
  constructor(
    private _router: Router,
    private _http: HttpClient,
    private _appSetting: AppSettings
  ) {}

  isLoginSubject = new BehaviorSubject<boolean>(this.hasToken());

  isLoggedIn(): Observable<boolean> {
    let islogin$ = this.isLoginSubject.asObservable().pipe(share());

    return islogin$;
  }

  login(form: NgForm): void {
    let account = this._appSetting.APIEndPoints.Account;

    let credentials = JSON.stringify(form.value);
    let request = this._http.post(account.Login, credentials);

    request.subscribe(
      resp => {
        // __::todo- add user roles, claims, toasts etc..

        console.log(resp);
        let token = (<any>resp).token;
        // console.log(this._jwtHelper.decodeToken(token));
        localStorage.setItem("token", token);

        this.isLoginSubject.next(true);

        toastmaker.createToast({
          type: "success",
          title: "Logged In",
          text: "Session created.",
        });

        this._router.navigate(["/veiw/dashboard/"]);
      },
      err => {
        localStorage.removeItem("token");

        this.isLoginSubject.next(false);
        this.handleError(err);
      }
    );
  }

  logout(): void {
    localStorage.removeItem("token");
    this.isLoginSubject.next(false);

    this._router.navigate(["/account/login"]);
  }

  private hasToken(): boolean {
    var token = localStorage.getItem("token");

    if (token && !this._jwtHelper.isTokenExpired(token)) {
      return true;
    } else {
      localStorage.removeItem("token");
      return false;
    }
  }

  verifyToken(): boolean {
    var token = localStorage.getItem("token");

    if (token && !this._jwtHelper.isTokenExpired(token)) {
      this.isLoginSubject.next(true);

      return true;
    } else {
      localStorage.removeItem("token");
      this.isLoginSubject.next(false);

      return false;
    }
  }

  handleError(err: HttpErrorResponse) {
    // let tittle = "Error";
    // let text = "success";

    toastmaker.createToast({
      type: "error",
      title: err.status,
      text: err.message,
    });
  }
}
