import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Member, Photo } from 'src/app/models/member';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HtmlParser } from '@angular/compiler';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { MembersService } from 'src/app/Services/members.service';

class ImageSnippet {
  constructor(public src: string, public file: File) {}
}

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  constructor(
    private http: HttpClient,
    private router: Router,
    private membersService: MembersService
  ) {}

  selectedFile: ImageSnippet | null = null;
  processFile(imageInput: any) {
    const file: File = imageInput.files[0];
    const reader = new FileReader();

    reader.addEventListener('load', (event: any) => {
      this.selectedFile = new ImageSnippet(event.target.result, file);

      const formData = new FormData();

      formData.append('file', this.selectedFile.file);

      return this.http
        .post<Photo>('http://localhost:5000/api/Users/add-photo', formData)
        .subscribe((photo: Photo) => {
          this.onReload(photo);
        });
    });

    reader.readAsDataURL(file);
  }

  @Input() member: Member | null = null;
  @Output() onReloadMember = new EventEmitter();
  @Output() onReloadPhotos = new EventEmitter();
  @Output() onDeletePhoto = new EventEmitter();

  setMainPhoto(photoId: number) {
    this.membersService.setMainPhoto(photoId).subscribe(() => {
      this.reloadPhotos(photoId);
    });
  }
  reloadPhotos(photoId: number) {
    this.onReloadPhotos.emit(photoId);
  }

  onReload(photo1: Photo) {
    this.onReloadMember.emit(photo1);
  }

  deletePhoto(photoId: number) {
    this.membersService.deletePhoto(photoId).subscribe((res) => {
      this.onDeletePhoto.emit(photoId);
    });
  }

  ngOnInit(): void {}
}
