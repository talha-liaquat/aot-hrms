import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
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

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService
) { 
    // redirect to home if already logged in
    // if (this.authenticationService.currentUserValue) { 
    //     this.router.navigate(['/']);
    // }
}

    // convenience getter for easy access to form fields
get f() { return this.registerForm.controls; }

ngOnInit() {
    this.registerForm = this.formBuilder.group({
      name: [null, Validators.required],  
      email: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required]        
    });    
}

  onRegisterClick(){
    this.submitted = true;

    console.log(this.f.name.value);
    console.log(this.f.email.value);
    console.log(this.f.username.value);
    console.log(this.f.password.value);

    // stop here if form is invalid
    if (this.registerForm.invalid) {
        return;
    }
  this.authenticationService.register(this.f.name.value, this.f.email.value, this.f.username.value, this.f.password.value)
      .pipe(first())
      .subscribe(
          data => {
            //this.router.navigate(['/']);
          },
          error => {
              this.error = error;
              console.log(error);
              this.loading = false;
              console.log(error);
          });
  }

}
