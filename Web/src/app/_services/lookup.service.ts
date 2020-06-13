import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { User } from '../_models';
import * as jwt_decode from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class LookupService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient) {
      this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
      this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
      return this.currentUserSubject.value;
  }

  create  (title: string, createdBy: string) {
      return this.http.post<any>(environment.apiBaseUrl + "v1/Lookups/skills", { title, createdBy }, {
          headers: new HttpHeaders({
               'Content-Type':  'application/json'
             })
        })
          .pipe(map(skill => {
              return skill;
          }));
  }

  getUserId(){
      var currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
      console.log(currentUserSubject.value["userId"]);
  }
}