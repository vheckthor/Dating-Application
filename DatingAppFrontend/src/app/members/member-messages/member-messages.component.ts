import { Component, OnInit, Input } from '@angular/core';
import { IMessage } from 'src/app/interfaces/message';
import { UsersService } from 'src/app/allservices/Users.service';
import { AuthService } from 'src/app/allservices/auth.service';
import { AlertifyService } from 'src/app/allservices/alertify.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() recipientId: string;
  messages: IMessage[];
  newMessage: any = {};

  constructor(private userService: UsersService, private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }
  loadMessages(){
    this.userService.getMessageThread(this.authService.decodedToken.nameid, this.recipientId)
    .subscribe( res => {
      this.messages = res;
    }, error => {
      this.alertify.error(error);
    });
  }

  sendMessage(){
    this.newMessage.recipientUniqueId = this.recipientId;
    this.userService.sendMessage(this.authService.decodedToken.nameid, this.newMessage).subscribe((res: IMessage) => {
      this.messages.push(res);
      this.newMessage.content = '';
    }, error => {
      this.alertify.error(error);
    });
  }

}
