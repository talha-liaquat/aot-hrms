import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './_services';
import { User } from './_models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  
  title = 'AOT HMRS Portal';
  currentUser: User;
  isAdmin: boolean;

  constructor(
      private router: Router,
      private authenticationService: AuthenticationService
  ) {
      this.authenticationService.currentUser.subscribe(x => this.currentUser = x);      
      this.authenticationService.isAdmin.subscribe(x => this.isAdmin = x);
    }
  

  ngOnInit() {
  }

  logout() {
      this.authenticationService.logout();
      this.router.navigate(['/login']);
  }
}