import { Component, OnInit, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/Services/members.service';
import { PresenceService } from 'src/app/Services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member | undefined;

  constructor(
    private memberService: MembersService,
    private toastr: ToastrService,
    public presence: PresenceService
  ) {}

  addLike(member: Member) {
    this.memberService.addLike(member.username).subscribe(() => {
      this.toastr.success('You have liked' + member.knownAs);
    });
  }

  ngOnInit(): void {
    console.log(this.member);
  }
}
