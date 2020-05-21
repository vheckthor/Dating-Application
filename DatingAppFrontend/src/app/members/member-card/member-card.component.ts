import { Component, OnInit, Input } from '@angular/core';
import { IUser } from 'src/app/interfaces/IUser';
import { UsersService } from 'src/app/allservices/Users.service';
import { AuthService } from 'src/app/allservices/auth.service';
import { AlertifyService } from 'src/app/allservices/alertify.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
@Input() user: IUser;
userId: string = this.authServices.decodedToken.nameid;
  constructor(private userServices: UsersService, private authServices: AuthService, 
              private alertify: AlertifyService) { }

  ngOnInit() {
  }
  sendLike(id: string){
    this.userServices.sendLike(this.userId, id).subscribe(data => {
      this.alertify.success('You have liked: ' + this.user.knownAs);
    }, error => {
      this.alertify.error(error);
    });
  }
}
