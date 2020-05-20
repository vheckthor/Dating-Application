import { Component, OnInit } from '@angular/core';
import { IUser } from '../../interfaces/IUser';
import { UsersService } from '../../allservices/Users.service';
import { AlertifyService } from '../../allservices/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { IPagination, PaginationResult } from 'src/app/interfaces/Pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

users: IUser[];
pagination: IPagination;
  constructor(private userServices: UsersService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
    this.users = data.users.result;
    this.pagination = data.users.pagination;
    });
  }

  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers(){
    this.userServices
    .getUsers(this.pagination.currentPage, this.pagination.itemsPerPage)
    .subscribe(
      (res: PaginationResult<IUser[]>) =>{
        this.users = res.result;
        this.pagination = res.pagination;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
}
