import { Component, OnInit } from '@angular/core';
import { AuthService } from './allservices/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'DatingAppFrontend';
  jwthelper = new JwtHelperService();
  constructor(private authServices: AuthService) {}


  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const userpix: string = localStorage.getItem('userpix');
    if (token){
      this.authServices.decodedToken = this.jwthelper.decodeToken(token);
    }
    if (userpix){
      this.authServices.currentUser = userpix;
      this.authServices.changeMemberPhoto(userpix);
    }
  }
}
