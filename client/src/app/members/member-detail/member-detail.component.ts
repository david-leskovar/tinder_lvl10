import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/models/member';
import { Message } from 'src/app/models/message';
import { MembersService } from 'src/app/Services/members.service';
import { MessageService } from 'src/app/Services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', { static: true }) memberTabs: TabsetComponent;

  activeTab: TabDirective;
  messages: Message[] = [];

  member: Member | null = null;
  constructor(
    private memberService: MembersService,
    private route: ActivatedRoute,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.member = data['member'];
    });

    this.route.queryParams.subscribe((params) => {
      params['tab'] ? this.selectTab(params['tab']) : this.selectTab(0);
    });
  }

  loadMember() {
    console.log(this.route.snapshot.paramMap.get('username'));
    this.memberService
      .getMember(this.route.snapshot.paramMap.get('username')!)
      .subscribe((member) => {
        console.log(member);
        this.member = member;
      });
  }

  loadMessages() {
    this.messageService
      .getMessageThread(this.member?.username!)
      .subscribe((messages) => {
        this.messages = messages;
      });
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      this.loadMessages();
    }
  }
}
