import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';
// import { JwtHelperService } from '@auth0/angular-jwt';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CanActivate, Router } from '@angular/router';
import { _sanitizeHtml } from '@angular/core/src/sanitization/html_sanitizer';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, public _auth: AuthService) {
  }

  canActivate() {
    let isValidToken = this._auth.verifyToken();
    if(isValidToken)
        return isValidToken;

    this.router.navigate(["account/login"]);
    return isValidToken;
  }
}
