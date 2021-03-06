import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { LookupService } from '../_services/lookup.service';
import { EmployeeService } from '../_services/employee.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

  public value: string[];
  public employees = [];

   // define the JSON of data
   public skills: { [key: string]: Object; }[];
    // maps the local data column to fields property
    public localFields: Object = { text: 'title', value: 'id' };
    // set the placeholder to MultiSelect Dropdown input element
    public localWaterMark: string = 'Please select your skill(s)';

    loading = false;
    submitted = false;
    returnUrl: string;
    email: string;
    title: string;
    error = '';
  
    constructor(
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private lookupService: LookupService,
      private employeeService: EmployeeService) { 
    }

ngOnInit() {
  var createdBy = JSON.parse(localStorage.getItem('currentUser'))["employeeId"];

    this.lookupService.getAll()
  .pipe(first())
  .subscribe(
      data => {
        this.skills = data;
      },
      error => {
          this.error = error;
          this.loading = false;
      });
  }

onSubmit(): void {

  this.employeeService.getEmployees(this.value[0])
  .pipe(first())
  .subscribe(
      data => {
        this.employees = data;
      },
      error => {
          this.error = error;
          this.loading = false;
      });
  
  }

}
