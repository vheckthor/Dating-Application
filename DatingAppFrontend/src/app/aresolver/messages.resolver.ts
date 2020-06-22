import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { IUser } from '../interfaces/IUser';
import { UsersService } from '../allservices/Users.service';
import { AlertifyService } from '../allservices/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IMessage } from '../interfaces/message';
import { AuthService } from '../allservices/auth.service';

@Injectable()
export class MessagesResolver implements Resolve<IMessage[]>{
    pageNumber = 1;
    pageSize = 6;
    messageContainer = 'Unread';

    constructor(private userServices: UsersService, private authService: AuthService,
                private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<IMessage[]>{
        return this.userServices.getMessages(this.authService.decodedToken.nameid,
            this.messageContainer, this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving messages');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

}
