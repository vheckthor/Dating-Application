import { Component, OnInit } from '@angular/core';
import { AuthService } from '../allservices/auth.service';
import { AlertifyService } from '../allservices/alertify.service';
import {Imodel} from '../interfaces/Imodel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  model: Imodel = {
    username: '',
    password: ''
  };
  photoLink = '';
  constructor(public authServices: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.authServices.currentPhotoUrl.subscribe(photopath =>{ this.photoLink = photopath; });
  }
  login(){
    this.authServices.login(this.model).subscribe(next => {
      this.alertify.success('logged in successfully');
    }, error => {
      console.log(error);
      if (error === 'Unauthorized'){
        this.alertify.error(error + ': password or username is Invalid');
        return;
      }
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn(){
    return this.authServices.loggedIn();
  }

  loggedOut(){
    localStorage.removeItem('token');
    localStorage.removeItem('userpix');
    this.authServices.decodedToken = null;
    this.authServices.currentUser = null;
    this.model.username = '';
    this.model.password = '';
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }
}
