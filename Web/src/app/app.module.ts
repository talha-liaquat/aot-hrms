import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule, routingComponents  } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from "ngx-spinner";
import { LoginComponent } from './login/login.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { RegisterComponent } from './register/register.component';
import { MygroupsComponent } from './mygroups/mygroups.component';
import { CreategroupComponent } from './creategroup/creategroup.component';
import { JoingroupComponent } from './joingroup/joingroup.component';

@NgModule({
  declarations: [
    AppComponent,
    routingComponents,
    LoginComponent,
    RegisterComponent,
    MygroupsComponent,
    CreategroupComponent,
    JoingroupComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }   
],
  bootstrap: [AppComponent]
})
export class AppModule { }
