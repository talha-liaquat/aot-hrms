import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { User } from '../_models';
import * as jwt_decode from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    login(username: string, password: string) {
        return this.http.post<any>(environment.apiBaseUrl + "v1/identity/authenticate", { username, password }, {
            headers: new HttpHeaders({
                 'Content-Type':  'application/json'
               })
          })
            .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes                
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }));
    }

    register(name: string, email: string, username: string, password: string) {
        return this.http.post<any>(environment.apiBaseUrl + "v1/identity/register", { name, email, username, password }, {
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
        var decoded = jwt_decode(currentUserSubject.value.Token); 
        console.log(decoded);
        return decoded[key];
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}