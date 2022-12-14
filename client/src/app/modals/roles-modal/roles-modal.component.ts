import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AuthService, User } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css'],
})
export class RolesModalComponent implements OnInit {
  @Input() updateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[];

  constructor(public bsModalRef: BsModalRef, public authService: AuthService) {}

  ngOnInit(): void {}

  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }
}
