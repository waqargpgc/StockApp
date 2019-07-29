import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../user.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {
  public addUser = {
    fname: '',
    lname: '',
    email: '',
    password: '',
    phone: '',
  }
  public AllUsers = [];
  public modalHeader:string;
  public Cpassword:string;
  public submitButton:string;
  @Input() type:string;
  constructor(public activeModal: NgbActiveModal, public _userService: UserService) { }

  ngOnInit() {
    debugger
   // this.fetchAllUser();
   if(this.type == 'add'){
    this.modalHeader = 'Add User';
    this.submitButton = 'Save';
   }
   else{
    this.modalHeader = 'Update User';
    this.submitButton = 'update'
   }
  }
  // get All user
  fetchAllUser(){
    // this._userService.getAllUser().subscribe(res=>{
    //   this.AllUsers = res;
    // })
  }
  // add user
  AddUser(type) {
  //   if (type == 'add') {
  //     this._userService.adduser(this.addUser).subscribe(res => {
  //       if (res.success) {
  //         alert(res.messag);
  //       }
  //       else {
  //         alert(res.messag);
  //       }
  //     },
  //       error => { });
  //   }
  //   else {
  //     this._userService.updateuser(this.addUser).subscribe(res => {
  //       if (res.success) {
  //         alert(res.messag);
  //       }
  //       else {
  //         alert(res.messag);
  //       }
  //     },
  //       error => { });
  //   }
  // }
  // // confirm password
  // ConfirmPass(value) {
  //   if (value != this.addUser.password) {
  //     alert('Password does not match')
  //   }
   }
}
