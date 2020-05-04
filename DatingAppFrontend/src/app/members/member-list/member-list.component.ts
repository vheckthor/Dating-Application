import { Component, OnInit } from '@angular/core';
import { IUser } from '../../interfaces/IUser';
import { UsersService } from '../../allservices/Users.service';
import { AlertifyService } from '../../allservices/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

users: IUser[];
  constructor(private userServices: UsersService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
    this.users = data.users;
    });
  }

}
