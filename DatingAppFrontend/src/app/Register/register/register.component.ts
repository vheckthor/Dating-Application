import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/app/allservices/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();

  model: any = {};

  constructor(private authservices: AuthService) {

  }

  ngOnInit() {
  }

  register(){
    this.authservices.register(this.model).subscribe(next => {
     console.log('Registration Successful');
    }, error => {
      console.log('Error occured please try again ');
    });
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
