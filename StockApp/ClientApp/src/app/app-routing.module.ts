
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AccountModule } from './account/account.module';

const routes: Routes = [
  { path: '', redirectTo: 'account/login', pathMatch: 'full' },
  { path: 'account', loadChildren: () => AccountModule }, // , canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'account/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
