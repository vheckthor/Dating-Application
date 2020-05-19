import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import {map} from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { IRegister } from '../interfaces/IRegister';
import { Imodel } from '../interfaces/Imodel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';

  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: string;
  photoUrlSubject = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrlSubject.asObservable();

constructor(private http: HttpClient) { }

changeMemberPhoto(photoUrl: string){
  this.photoUrlSubject.next(photoUrl);
}

login(model: Imodel){
  return this.http.post(this.baseUrl + 'login', model)
  .pipe(
    map((response: any) => {
      const user = response;
      if (user) {
        localStorage.setItem('token', user.token);
        localStorage.setItem('userpix', user.photoUrl);
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
        this.currentUser = user.photoUrl;
        this.changeMemberPhoto(this.currentUser);
      }
    })
  );
}

register(model: IRegister){
  return this.http.post(this.baseUrl + 'register', model )
  .pipe(map((response: any) => {

  }));
}

loggedIn(){
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token);

}

}
