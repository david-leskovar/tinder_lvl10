import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from '../Services/auth.service';
import { User } from '../Services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  constructor(private authService: AuthService) {}

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
          console.log(error);
          this.error = error.error;
          console.log(this.error);
          this.signInLoading = false;
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
