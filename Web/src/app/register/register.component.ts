import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first, switchMap } from 'rxjs/operators';
import { AuthenticationService } from '../_services';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';
  get f() { return this.registerForm.controls; }
  name: string;
  email: string;
  state: string

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService
) { 
  console.log('Called Constructor');
  this.route.queryParams.subscribe(params => {
      this.name = params['name'];
      this.email = params['email'];
      this.state = params['state'];
  });
}

ngOnInit() {
    this.registerForm = this.formBuilder.group({
      name: [this.name, Validators.required],  
      email: [this.email, Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required]        
    });    
}

  onRegisterClick(){
    this.submitted = true;

    if (this.registerForm.invalid) {
        return;
    }
    
    this.authenticationService.register(this.f.name.value, this.f.email.value, this.f.username.value, this.f.password.value, this.state)
      .pipe(first())
      .subscribe(
          data => {
            this.router.navigate(['/login']);
          },
          error => {
              this.error = error;
              this.loading = false;
          });
  }

}
