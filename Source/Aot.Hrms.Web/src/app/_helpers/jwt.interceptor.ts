import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthenticationService } from '../_services/authentication.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var currentUser = localStorage.getItem('currentUser');
        if(currentUser != null){
        var token = JSON.parse(currentUser)["token"];
        if (token != null) {
            request = request.clone({
                setHeaders: {                    
                    Authorization: 'Bearer ' + token
                }
            });
        }
    }

        return next.handle(request);
    }
}