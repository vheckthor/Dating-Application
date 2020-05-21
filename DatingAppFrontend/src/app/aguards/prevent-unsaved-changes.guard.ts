import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable()
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
    canDeactivate(
        component: MemberEditComponent,
    ){
        if (component.editForm.dirty){
            return confirm('Are you sure you want to continue? Any unsaved changes would be lost');
        }
        return true;
    }
}