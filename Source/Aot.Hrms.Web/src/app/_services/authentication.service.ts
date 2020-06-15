import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { User } from '../_models';
import * as jwt_decode from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<User>;
    private isAdminSubject: BehaviorSubject<boolean>;

    public currentUser: Observable<User>;
    public isAdmin: Observable<boolean>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();

        this.isAdminSubject = new BehaviorSubject<boolean>(this.getClaims('is-admin'));
        this.isAdmin = this.isAdminSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    login(username: string, password: string) {
        return this.http.post<any>("http://localhost:61653/api/v1/identity/authenticate", { username, password }, {
            headers: new HttpHeaders({
                 'Content-Type':  'application/json'
               })
          })
            .pipe(map(user => {
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
                let isAdmin = this.getClaims("is-admin");
                this.isAdminSubject.next(isAdmin == "true");
                return user;
            }));
    }

    register(name: string, email: string, username: string, password: string, state: string) {
        return this.http.post<any>("http://localhost:61653/api/v1/identity/register", { name, email, username, password, state}, {
            headers: new HttpHeaders({
                 'Content-Type':  'application/json'
               })
          })
            .pipe(map(user => {
                return true;
            }));
    }

    getClaims(key: string){        
        var currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        if(currentUserSubject.value){
            var decoded = jwt_decode(currentUserSubject.value.token);
            return decoded[key];
        }
        return null;
    }

    logout() {
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}