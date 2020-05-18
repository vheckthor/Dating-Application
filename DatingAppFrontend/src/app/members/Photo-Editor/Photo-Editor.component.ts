import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IPhoto } from 'src/app/interfaces/IPhoto';
import {FileUploader} from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/allservices/auth.service';
import { UsersService } from 'src/app/allservices/Users.service';
import { AlertifyService } from 'src/app/allservices/alertify.service';

@Component({
  selector: 'app-Photo-Editor',
  templateUrl: './Photo-Editor.component.html',
  styleUrls: ['./Photo-Editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: IPhoto[];
  @Output() getChangedPhoto = new EventEmitter<string>();


  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMainPhoto: IPhoto;

  constructor(private authService: AuthService, private userService: UsersService, private alertify: AlertifyService) {

  }

  ngOnInit() {
    this.initializeFileUploader();
  }
  fileOverBase(e: any): void{
    this.hasBaseDropZoneOver = e;
  }
  initializeFileUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response){
        const res: IPhoto = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          photoUniqueIdentifier: res.photoUniqueIdentifier,
          isMain: res.isMain
        };
        this.photos.push(photo);
      }
    };
  }
  setMainPhoto(photo: IPhoto){
    this.userService.setMainPhoto(this.authService.decodedToken.nameid, photo.photoUniqueIdentifier).subscribe(
      () => {
        this.currentMainPhoto = this.photos.filter(x => x.isMain === true)[0];
        this.currentMainPhoto.isMain = false;
        photo.isMain = true;
        // this.getChangedPhoto.emit(photo.url);
        this.authService.changeMemberPhoto(photo.url);
        this.authService.currentUser = photo.url;
        localStorage.setItem('userpix', photo.url);
        this.alertify.success('Profile picture changed successfully');
      }),
      error => { this.alertify.error(error); };
  }

  deletePhoto(id: string){
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      this.userService.deletePhoto(this.authService.decodedToken.nameid, id).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.photoUniqueIdentifier === id), 1 );
        this.alertify.success('Photo has been deleted');
      }, error => { this.alertify.error('Failed to delete the photo');
    });
    });
  }
}
