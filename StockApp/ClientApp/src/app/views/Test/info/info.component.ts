import { _sanitizeHtml } from '@angular/core/src/sanitization/html_sanitizer';
import { AppSettings } from './../../../shared/app.settings';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'stk-info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.scss']
})
export class InfoComponent implements OnInit {
  data: any;
   constructor(private http: HttpClient, private _appSetting: AppSettings) {}

  ngOnInit() {
    this.loadEPInfo();
  }

  loadEPInfo(){
    let sampleEP = this._appSetting.APIEndPoints.EndPointsInfo;
    let req = this.http.get(sampleEP.EndPoints);

    req.subscribe(
      data => {
        this.data = data;
        console.log(data)
      },
      err => console.log(err)
    )
  }

  loadSampleUsers(){
    let accountEP = this._appSetting.APIEndPoints.Account;

    let req = this.http.get(accountEP.GetUsers);

    req.subscribe(
      data => {
        this.data = data;
        console.log(data)
      },
      err => console.log(err)
    )
  }

  loadSampleRoles(){
    let accountEP = this._appSetting.APIEndPoints.Account;
    let req = this.http.get(accountEP.GetRoles);

    req.subscribe(
      data => {
        this.data = data;
        console.log(data)
      },
      err => console.log(err)
    )
  }

}
