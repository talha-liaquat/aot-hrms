import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component'
import { RegisterComponent } from './register/register.component'
import { MygroupsComponent } from './mygroups/mygroups.component'
import { CreategroupComponent } from './creategroup/creategroup.component'
import { JoingroupComponent } from './joingroup/joingroup.component'
import { AuthGuard } from './_helpers';

const routes: Routes = [  
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'mygroups', component: MygroupsComponent },
  { path: 'creategroup', component: CreategroupComponent },
  { path: 'joingroup', component: JoingroupComponent },  
   { path: '**', redirectTo: 'creategroup' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [
  
];

export const appRoutingModule = RouterModule.forRoot(routes);