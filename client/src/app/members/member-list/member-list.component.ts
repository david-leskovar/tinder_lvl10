import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/models/member';
import { Pagination } from 'src/app/models/pagination';
import { UserParams } from 'src/app/models/userParams';
import { AuthService, User } from 'src/app/Services/auth.service';
import { MembersService } from 'src/app/Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];
  pagination: Pagination;
  userParams: UserParams;
  user: User;
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  constructor(
    private memberService: MembersService,
    private authService: AuthService
  ) {
    this.user = this.authService!.currentUser!;

    this.userParams = new UserParams(this.user);
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.userParams).subscribe((response) => {
      this.members = response.results;
      this.pagination = response.pagination;
    });
  }
  resetFilters() {
    this.userParams = new UserParams(this.user);
    this.loadMembers();
  }
}
