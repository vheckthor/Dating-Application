import { Component, OnInit } from '@angular/core';
import { IUser } from '../interfaces/IUser';
import { IPagination, PaginationResult } from '../interfaces/Pagination';
import { AuthService } from '../allservices/auth.service';
import { UsersService } from '../allservices/Users.service';
import { AlertifyService } from '../allservices/alertify.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-like-list',
  templateUrl: './like-list.component.html',
  styleUrls: ['./like-list.component.css']
})
export class LikeListComponent implements OnInit {

  users: IUser[];
  pagination: IPagination;
  likesParam: string;

  constructor(private authServices: AuthService, private routes: ActivatedRoute,
              private userServices: UsersService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.routes.data.subscribe( data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
    });
    this.likesParam = 'Likers';
  }

  loadUsers(){
    this.userServices
    .getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.likesParam)
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

  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

}
