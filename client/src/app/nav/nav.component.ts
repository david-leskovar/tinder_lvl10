import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { delay } from 'rxjs';
import { AuthService } from '../Services/auth.service';
import { User } from '../Services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  mobileOpen: boolean = false;
  openForMobile() {
    if (this.mobileOpen) {
      this.mobileOpen = false;
    } else {
      this.mobileOpen = true;
    }
  }

  error: string = '';
  signInLoading: boolean = false;

  onNavEnter() {
    this.navOpen = true;
  }
  onNavLeave() {
    this.navOpen = false;
  }

  navOpen: boolean = false;
  currentUser: User | null = null;

  onLogout() {
    this.authService.logout();
  }

  onSubmit(form: NgForm) {
    this.signInLoading = true;
    if (!form.valid) {
      console.log('not valid!');
      return;
    }

    this.authService
      .login({
        username: form.value.username,
        password: form.value.password,
      })

      .subscribe(
        (data) => {
          this.signInLoading = false;
        },
        (error) => {
          setTimeout(() => {
            this.toastr.error(error.error);
            this.signInLoading = false;
          }, 1500);
        }
      );
  }

  ngOnInit(): void {
    this.currentUser = this.authService.currentUser;
    this.authService.userChanged.subscribe((user: User | null) => {
      this.currentUser = user;
    });
  }
}
