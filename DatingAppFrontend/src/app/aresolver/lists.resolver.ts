import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { IUser } from '../interfaces/IUser';
import { UsersService } from '../allservices/Users.service';
import { AlertifyService } from '../allservices/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ListsResolver implements Resolve<IUser[]>{
    pageNumber = 1;
    pageSize = 6;
    likesParam = 'Likers';

    constructor(private userServices: UsersService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<IUser[]>{
        return this.userServices.getUsers(this.pageNumber, this.pageSize, null, this.likesParam).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

}
