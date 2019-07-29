import { Component, OnInit } from '@angular/core';
import { NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddUserComponent } from './add-user/add-user.component';

@Component({
  templateUrl: 'test.component.html'
})
export class TestComponent implements OnInit {
  constructor(private _modalService: NgbModal){}
  ngOnInit(): void {}


  deleteUser(){

  }
  ngbModalOptionsSmall: NgbModalOptions = {
    backdrop: 'static',
    keyboard: false,
    size:'lg'
  };
  openAddUserModal() {
    const modalRef = this._modalService.open(AddUserComponent, this.ngbModalOptionsSmall);
    modalRef.componentInstance.type = 'add';
    modalRef.result.then((result) => {
    })
  }
  openUpdaeUserModal(user) {
    const modalRef = this._modalService.open(AddUserComponent, this.ngbModalOptionsSmall);
    modalRef.componentInstance.type = 'update';
    modalRef.result.then((result) => {
    })
  }
}
