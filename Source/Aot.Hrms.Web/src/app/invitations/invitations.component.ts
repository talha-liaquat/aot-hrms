import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { EmployeeService } from '../_services/employee.service';

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.scss']
})

export class InvitationsComponent implements OnInit {
  loading = false;
  submitted = false;
  returnUrl: string;
  email: string;
  name: string;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private employeeService: EmployeeService) { 
  }

  ngOnInit() {   
  }

  OnCreateClick(){
  
    this.employeeService.invite(this.name, this.email, false)
        .pipe(first())
        .subscribe(
            data => {
                this.router.navigate(['/dashboard']);
            },
            error => {
                this.error = error;
                this.loading = false;
            });
  }

}
