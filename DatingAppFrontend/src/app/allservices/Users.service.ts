import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser } from '../interfaces/IUser';


@Injectable({
  providedIn: 'root'
})
export class UsersService {
baseUrl = environment.apiUrl;
constructor(private http: HttpClient) { }

getUsers(): Observable<IUser[]>{
  return  this.http.get<IUser[]>(this.baseUrl + 'users');
}

getUser(id): Observable<IUser>{
  return this.http.get<IUser>(this.baseUrl + 'users/' + id);
}

updateUser(id, user: IUser){
  return this.http.put<IUser>(this.baseUrl + 'users/' + id, user);
}

}
