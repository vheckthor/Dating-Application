import {Routes} from '@angular/router';
import { HomeComponent } from './home/home/home.component';
import { LikeListComponent } from './like-list/like-list.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './member-list/member-list.component';
import { AuthGuard } from './aguards/auth.guard';




export const appRoutes: Routes = [
{path: '', component: HomeComponent},
{
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
    {path: 'members', component: MemberListComponent},
    {path: 'messages', component: MessagesComponent},
    {path: 'lists', component: LikeListComponent},

    ]
},
{path: '**', redirectTo: '', pathMatch: 'full'}
];
