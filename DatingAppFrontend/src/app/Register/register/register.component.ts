import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/app/allservices/auth.service';
import { AlertifyService } from 'src/app/allservices/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();

  model: any = {};

  constructor(private authservices: AuthService, private alertify: AlertifyService) {

  }

  ngOnInit() {
  }

  register(){
    this.authservices.register(this.model).subscribe(next => {
     this.alertify.success('Registration Successful');
    }, error => {
      this.alertify.error(error);
    });
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}