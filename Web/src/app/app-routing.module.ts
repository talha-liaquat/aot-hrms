import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component'
import { RegisterComponent } from './register/register.component'
import { InvitationsComponent } from './invitations/invitations.component'
import { SkillsComponent } from './skills/skills.component'
import { SearchComponent } from './search/search.component'
import { DashboardComponent } from './dashboard/dashboard.component'
import { MySkillsComponent } from './my-skills/my-skills.component'
import { AuthGuard } from './_helpers';

const routes: Routes = [  
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: DashboardComponent },  
  { path: 'invitations', component: InvitationsComponent },  
  { path: 'skills', component: SkillsComponent },  
  { path: 'search', component: SearchComponent },  
  { path: 'mySkills', component: MySkillsComponent },  
   { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

export const routingComponents = [
  
];

export const appRoutingModule = RouterModule.forRoot(routes);