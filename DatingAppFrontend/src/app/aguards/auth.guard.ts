import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AlertifyService } from '../allservices/alertify.service';
import { AuthService } from '../allservices/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authServices: AuthService, private router: Router, private alertify: AlertifyService) {}

  canActivate(): boolean {
    if (this.authServices.loggedIn()){
      return true;
    }
    this.alertify.error('Access denied unable to login');
    this.router.navigate(['/home']);
    return false;
  }

}
