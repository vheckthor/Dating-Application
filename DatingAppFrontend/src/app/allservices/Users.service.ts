import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser } from '../interfaces/IUser';
import { IPhoto } from '../interfaces/IPhoto';
import { PaginationResult } from '../interfaces/Pagination';
import { map } from 'rxjs/operators';
import { IMessage } from '../interfaces/message';



@Injectable({
  providedIn: 'root'
})
export class UsersService {
baseUrl = environment.apiUrl;
constructor(private http: HttpClient) { }

getUsers(page?, itemsPerPage?, userParams?, likesParam?): Observable<PaginationResult<IUser[]>>{
  const paginationResult: PaginationResult<IUser[]> = new PaginationResult<IUser[]>();
  let params = new HttpParams();
  if (page != null && itemsPerPage != null){
    params = params.append('pageNumber',page);
    params = params.append('pageSize', itemsPerPage);
  }
  if (userParams != null){
    params = params.append('minAge',  userParams.minAge == null ? 18 : userParams.minAge);
    params = params.append('maxAge', userParams.maxAge == null ? 100 : userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
  }

  if (likesParam === 'Likers'){
    params = params.append('likers', 'true');
  }
  if (likesParam === 'Likees'){
    params = params.append('likees', 'true');
  }
  return  this.http.get<IUser[]>(this.baseUrl + 'users', {
   observe: 'response', params
  }).pipe(
    map(response => {
      paginationResult.result = response.body;
      if (response.headers.get('Pagination') != null){
        paginationResult.pagination = JSON.parse(response.headers.get('Pagination'))
      }
      return paginationResult;
    })
  );
}

getUser(id): Observable<IUser>{
  return this.http.get<IUser>(this.baseUrl + 'users/' + id);
}

updateUser(id, user: IUser){
  return this.http.put<IUser>(this.baseUrl + 'users/' + id, user);
}

setMainPhoto(userId: string, id: string){
  return this.http.post<IPhoto>(this.baseUrl + 'users/' + userId + '/Photos/' + id + '/setMain', {});
}

deletePhoto(userId: string, id: string){
  return this.http.delete(this.baseUrl + 'users/' + userId + '/Photos/' + id);
}

sendLike(id: string, recipientId: string){
  return this.http.post(this.baseUrl + 'users/' + id + '/like/' + recipientId, {});
}

getMessages(userId: string, messageContainer: string, page?, itemsPerPage?){
  const paginatedResult: PaginationResult<IMessage[]> = new PaginationResult<IMessage[]>();
  let params = new HttpParams();
  params = params.append('MessageContainer', messageContainer);
  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }
  return this.http.get<IMessage[]>(this.baseUrl + 'users/' + userId + '/messages' , {observe: 'response', params})
  .pipe(
    map(response => {
      paginatedResult.result = response.body;
      if ( response.headers.get('Pagination') !== null){
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
      }
      return paginatedResult;
    })
  );
}
getMessageThread(userId: string, recipientId: string){
  return this.http.get<IMessage[]>(this.baseUrl + 'users/' + userId + '/messages/thread/' + recipientId);
}
sendMessage(userId: string, message: IMessage){
  return this.http.post(this.baseUrl + 'users/' + userId + '/messages', message);
}
}
