import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../app/_models/user';
import * as jwt_decode from 'jwt-decode';
import { environment } from './../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private httpClient: HttpClient) {
      this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
      this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
      return this.currentUserSubject.value;
  }


  create(title: string, token: string) {
    return this.httpClient.post("http://localhost:61653/api/v1/Group", { title }, {
      headers: new HttpHeaders({
           'Content-Type':  'application/json',
           'Authorization': 'Bearer ' + token
         })
    });
}
}
