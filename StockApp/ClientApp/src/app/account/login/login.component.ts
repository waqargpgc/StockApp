import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: 'login.component.html'
})
export class LoginComponent {
  password: any = "Pass@123";
  username: any = "superadmin";
  constructor(private _authService: AuthService) { }
  login(form: NgForm) {
    this._authService.login(form);
  }
}
