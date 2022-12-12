import { Component, OnInit } from '@angular/core';
import { Member } from '../models/member';
import { MembersService } from '../Services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
})
export class ListsComponent implements OnInit {
  members: Partial<Member[]>;
  predicate = 'liked';

  constructor(private memberService: MembersService) {}

  loadLikes() {
    this.memberService.getLikes(this.predicate).subscribe((response) => {
      this.members = response;
      console.log(this.members);
    });
  }

  ngOnInit(): void {
    this.loadLikes();
  }
}
