import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Member, Photo } from 'src/app/models/member';
import { AuthService, User } from 'src/app/Services/auth.service';
import { MembersService } from 'src/app/Services/members.service';
import { MemberListComponent } from '../member-list/member-list.component';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | null = null;

  constructor(
    private authService: AuthService,
    private membersService: MembersService,
    private toastr: ToastrService
  ) {}

  loadingUser: boolean = false;
  user: User | null = null;
  member: Member | null = null;

  ngOnInit(): void {
    // Add it to the DOM

    if (this.authService.currentUser) {
      this.user = this.authService.currentUser;
      this.membersService.getMember(this.user.username).subscribe((member) => {
        this.member = member;
      });
    }
  }

  reloadMember(photo: Photo) {
    if (!this.member?.photoURL) {
      this.member!.photoURL = photo.url;
    }

    this.member?.photos.push(photo);
  }

  reloadPhotos(photoId1: number) {
    let old = this.member?.photos.find((photo) => photo.isMain);
    old!.isMain = false;

    let newMain = this.member?.photos.find(
      (photo: Photo) => photo.id === photoId1
    );
    newMain!.isMain = true;

    this.member!.photoURL! = newMain!.url;
  }

  deletePhoto(photoID: number) {
    this.member!.photos = this.member!.photos!.filter(
      (photo) => photo.id !== photoID
    );
  }

  updateMember() {
    this.membersService.updateMember(this.member!).subscribe(() => {
      this.editForm?.reset(this.member);
      this.toastr.success('Profile edited successfully');
    });
  }
}
