import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { LookupService } from '../_services/lookup.service';
import { EmployeeService } from '../_services/employee.service';

@Component({
  selector: 'app-my-skills',
  templateUrl: './my-skills.component.html',
  styleUrls: ['./my-skills.component.scss']
})
export class MySkillsComponent implements OnInit {

    public value: string[];


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

    this.employeeService.getSkills( createdBy)
    .pipe(first())
    .subscribe(
        data => {
          var tempArr = data.map(function(a) {return a.id;});
          console.log(tempArr);
          this.value = tempArr;
        },
        error => {
            this.error = error;
            this.loading = false;
        });

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

  onSubmit(form: ngForm): void {
    console.log(form.value);
    
    var createdBy = JSON.parse(localStorage.getItem('currentUser'))["employeeId"];
    
    this.employeeService.addSkills(createdBy, form.value.title)
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

}
