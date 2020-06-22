import {Routes} from '@angular/router';
import { HomeComponent } from './home/home/home.component';
import { LikeListComponent } from './like-list/like-list.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { AuthGuard } from './aguards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './aresolver/member-detail.resolver';
import { MemberListResolver } from './aresolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './aresolver/member-editresolver';
import { PreventUnsavedChangesGuard } from './aguards/prevent-unsaved-changes.guard';
import { ListsResolver } from './aresolver/lists.resolver';
import { MessagesResolver } from './aresolver/messages.resolver';




export const appRoutes: Routes = [
{path: '', component: HomeComponent},
{
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
    {path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver}},
    {path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver}},
    {path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChangesGuard]},
    {path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver}},
    {path: 'lists', component: LikeListComponent, resolve: {users: ListsResolver}},

    ]
},
{path: '**', redirectTo: '', pathMatch: 'full'}
];
