import { Component, OnInit } from '@angular/core';
import { AuthService } from '../allservices/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  model: any = {};
  constructor(private authServices: AuthService) { }

  ngOnInit() {
  }
  login(){
    this.authServices.login(this.model).subscribe(next => {
      console.log('logged in successfully');
    }, error => {
      console.log('Failed to login');
    });
  }

  loggedIn(){
    const token = localStorage.getItem('token');
    return !!token;
  }

  loggedOut(){
    localStorage.removeItem('token');
    console.log('logged out');
  }
}
