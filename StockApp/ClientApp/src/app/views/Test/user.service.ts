import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { AppSettings } from '../../shared/app.settings';
import { AuthService } from '../../services/auth.service';
import 'rxjs/add/operator/map';
@Injectable()
export class UserService {
    constructor(private _http: Http,public _authService:AuthService) { }
//get all user
getAllUser() {
    // return this._http.get(AppSettings.API_ENDPOINT + 'api/=',
    //     { headers: this._authService.getRequestHeader() }).map((res: Response) => res.json());
}
    //add user
    adduser(data) {
        // return this._http.post(AppSettings.API_ENDPOINT + 'api/=' + data,
        //     { headers: this._authService.getRequestHeader() }).map((res: Response) => res.json());
    }
    //update user
    updateuser(data) {
        // return this._http.post(AppSettings.API_ENDPOINT + 'api/=' + data,
        //     { headers: this._authService.getRequestHeader() }).map((res: Response) => res.json());
    }
}
