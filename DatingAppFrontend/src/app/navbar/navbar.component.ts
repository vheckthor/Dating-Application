import { Component, OnInit } from '@angular/core';
import { AuthService } from '../allservices/auth.service';
import { AlertifyService } from '../allservices/alertify.service';
import {Imodel} from '../interfaces/Imodel';

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
  constructor(public authServices: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
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
    });
  }

  loggedIn(){
    return this.authServices.loggedIn();
  }

  loggedOut(){
    localStorage.removeItem('token');
    this.model.username = '';
    this.model.password = '';
    this.alertify.message('logged out');
  }
}
