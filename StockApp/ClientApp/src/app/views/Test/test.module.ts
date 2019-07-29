import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TestComponent } from './test.component';
import { Routes, RouterModule } from '@angular/router';
import { AddUserComponent } from './add-user/add-user.component';
import { UserService } from './user.service';
import { Http, HttpModule } from '@angular/http';
import { InfoComponent } from './info/info.component';
const routes: Routes = [
  {
    path: '',
    component: TestComponent,
    data: {
      title: 'Test'
    }
  },
  {
    path: 'play',
    component: InfoComponent,
    data: {
      title: 'Info'
    }
  },
];
@NgModule({
  imports: [
    HttpModule,
    CommonModule,
    NgbModule,
    FormsModule,
    BsDropdownModule,
    RouterModule.forChild(routes),
    ButtonsModule.forRoot()
  ],
  declarations: [TestComponent,AddUserComponent, InfoComponent],
  exports:[],
  entryComponents:[AddUserComponent],
  providers:[UserService]
})
export class TestModule { }
