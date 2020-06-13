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
export class EmployeeService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient) {
      this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
      this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
      return this.currentUserSubject.value;
  }

  invite(name: string, email: string, isAdmin: boolean) {
      return this.http.post<any>(environment.apiBaseUrl + "v1/Employees", { name, email, isAdmin }, {
          headers: new HttpHeaders({
               'Content-Type':  'application/json'
             })
        })
          .pipe(map(employee => {
              return employee;
          }));
  }

  getClaims(key: string){
      var currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
      var decoded = jwt_decode(currentUserSubject.value.Token); 
      console.log(decoded);
      return decoded[key];
  }
}