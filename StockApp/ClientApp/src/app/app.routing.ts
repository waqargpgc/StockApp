import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { P404Component } from './views/error/404.component';


export const routes: Routes = [
  { path: '', redirectTo: 'account/login', pathMatch: 'full' },
  //{ path: 'account', loadChildren: () => AccountModule }, // , canActivate: [AuthGuard] },
 // { path: 'veiw', loadChildren: () => VeiwsModule }, // , canActivate: [AuthGuard] },
  { path: 'account', loadChildren:'./account/account.module#AccountModule'}, // , canActivate: [AuthGuard] },
  { path: 'veiw', loadChildren:'./views/veiws.module#VeiwsModule'}, // , canActivate: [AuthGuard] },
  { path: '**', component: P404Component }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
