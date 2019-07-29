import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class AppSettings {
  // public  Root = "https://stkapp.azurewebsites.net/api/";
  public Root = "https://plyxtock.herokuapp.com/api/";

  public APIEndPoints = {
    "Account": {
      "Login": this.Root + "auth/login",
      "Register": this.Root + "account/register",

      "CreateUser": this.Root + "account/createuser",
      "UpdateUser": this.Root + "account/updateuser",
      "DeleteUser": this.Root + "account/deleteuser", // deleteuser/id
      "GetUsers": this.Root + "account/getusers",
      "GetSingleUser": this.Root + "account/getusers", // getusers/[id,username]

      "CreateRole": this.Root + "account/createrole",
      "DeleteRole": this.Root + "account/deleterole", // deleterole/[id, name]
      "UpdateRole": this.Root + "account/updaterole",
      "GetRoles": this.Root + "account/getroles",
      "GetSingleRole": this.Root + "account/getroles"// getroles/[id, name]
    },
    "EndPointsInfo": {
      "EndPoints": this.Root + "endpoints"
    }
  };

}
