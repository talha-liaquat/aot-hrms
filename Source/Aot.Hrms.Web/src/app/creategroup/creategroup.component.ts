import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { GroupService } from '../group.service';

@Component({
  selector: 'app-creategroup',
  templateUrl: './creategroup.component.html',
  styleUrls: ['./creategroup.component.scss']
})
export class CreategroupComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  groupTitle: string;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private groupService: GroupService
) { 

}

  ngOnInit() {   
  }

  OnCreateClick(){
    this.submitted = true;
    
    this.loading = true;
    this.groupService.create(this.groupTitle, this.groupService.currentUserValue.token)
        .pipe(first())
        .subscribe(
            data => {
                this.router.navigate(['/mygroups']);
            },
            error => {
                this.error = error;
                this.loading = false;
            });
  }

}
