import { BrowserModule, HammerGestureConfig, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule, Pipe, PipeTransform } from '@angular/core';
import {BsDropdownModule} from 'ngx-bootstrap/dropdown';
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker'
import {TabsModule} from 'ngx-bootstrap/tabs';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthGuard } from './aguards/auth.guard';
import { UsersService } from './allservices/Users.service';
import { MemberDetailResolver } from './aresolver/member-detail.resolver';
import { AlertifyService } from './allservices/alertify.service';
import {NgxGalleryModule} from 'ngx-gallery-9';
import {NgxIntlTelInputModule} from 'ngx-intl-tel-input';
import {PaginationModule} from 'ngx-bootstrap/pagination';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { AuthService } from './allservices/auth.service';
import { HomeComponent } from './home/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RegisterComponent } from './Register/register/register.component';
import { ErrorInterceptorProvider } from './allservices/error.interceptor';
import { MemberListComponent } from './members/member-list/member-list.component';
import { LikeListComponent } from './like-list/like-list.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtModule } from '@auth0/angular-jwt';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListResolver } from './aresolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './aresolver/member-editresolver';
import { PreventUnsavedChangesGuard } from './aguards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './members/Photo-Editor/Photo-Editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import {TimeAgoPipe} from 'time-ago-pipe';



export function tokenGetter(){
  return localStorage.getItem('token');
}


export class CustomerHammerConfig extends HammerGestureConfig{
   overrides = {
      pinch: {enable: false},
      rotate: {enable: false}
   };
};


@Pipe({
 name: 'timeAgo',
 pure: false
})
export class TimeAgoExtendsPipe extends TimeAgoPipe implements PipeTransform{
   // transform(value: any): string {
   //    return value;
   // };
}

@NgModule({
   declarations: [
      AppComponent,
      NavbarComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      LikeListComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      TimeAgoExtendsPipe
      //TimeAgoPipe
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      NgxGalleryModule,
      HttpClientModule,
      FileUploadModule,
      ReactiveFormsModule,
      FormsModule,
      RouterModule.forRoot(appRoutes),
      PaginationModule.forRoot(),
      BsDropdownModule.forRoot(),
      NgxIntlTelInputModule,
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot(),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AuthGuard,
      PreventUnsavedChangesGuard,
      UsersService,
      MemberDetailResolver,
      MemberListResolver,
      AlertifyService,
      {provide: HAMMER_GESTURE_CONFIG, useClass: CustomerHammerConfig},
      MemberEditResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
