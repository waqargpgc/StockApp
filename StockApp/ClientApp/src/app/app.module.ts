import { TopNavComponent } from './skeleton/top-nav/top-nav.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { AppComponent } from './app.component';

// Import containers
//import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';

// const APP_CONTAINERS = [
//   DefaultLayoutComponent
// ];

// import {
//   AppAsideModule,
//   AppBreadcrumbModule,
//   AppHeaderModule,
//   AppFooterModule,
//   AppSidebarModule,
// } from '@coreui/angular';

// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts/ng2-charts';
//import { RegisterComponent } from './account/register/register.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AccountModule } from './account/account.module';
import { VeiwsModule } from './views/veiws.module';
import { Http, HttpModule } from '@angular/http';
import { UserService } from './views/Test/user.service';
import { FooterComponent } from './skeleton/footer/footer.component';
import { SideNavComponent } from './skeleton/side-nav/side-nav.component';
import { ShellComponent } from './skeleton/shell/shell.component';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    // AppAsideModule,
    // AppBreadcrumbModule.forRoot(),
    // AppFooterModule,
    // AppHeaderModule,
    // AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ChartsModule,
    AccountModule,
    VeiwsModule
  ],
  declarations: [
    AppComponent,
   // DefaultLayoutComponent,
    //...APP_CONTAINERS,
    P404Component,
    P500Component,
    FooterComponent,
    SideNavComponent,
    ShellComponent,
    TopNavComponent
  ],
  providers: [{
    provide: LocationStrategy,
    useClass: HashLocationStrategy
  },HttpModule],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
