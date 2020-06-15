import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { LookupService } from '../_services/lookup.service';

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss']
})
export class SkillsComponent implements OnInit {
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
    private lookupService: LookupService) { 
  }

  ngOnInit() {   
  }

  OnCreateClick(){
    var createdBy = JSON.parse(localStorage.getItem('currentUser'))["userId"];

    this.lookupService.create(this.title, createdBy)
        .pipe(first())
        .subscribe(
            data => {
                this.router.navigate(['/skills']);
            },
            error => {
                this.error = error;
                this.loading = false;
            });
  }

}
