import { Component, OnInit } from '@angular/core';
import { IMessage } from '../interfaces/message';
import { IPagination, PaginationResult } from '../interfaces/Pagination';
import { UsersService } from '../allservices/Users.service';
import { AuthService } from '../allservices/auth.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../allservices/alertify.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: IMessage[];
  pagination: IPagination;
  messageContainer = 'Unread';
  constructor(private userService: UsersService, private authService: AuthService, 
              private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data.messages.result;
      this.pagination = data.messages.pagination;
    });
  }
  loadMessages(){
    this.userService.getMessages(this.authService.decodedToken.nameid, this.messageContainer, this.pagination.currentPage,
      this.pagination.itemsPerPage)
    .subscribe((res: PaginationResult<IMessage[]>) => {
      this.messages = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }

}
