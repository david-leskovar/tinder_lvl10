import { HttpClient } from '@angular/common/http';
import { Token } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, delay, Subject, tap, throwError } from 'rxjs';

export interface UserLogin {
  username: string;
  password: string;
}

export interface User {
  username: string;
  token: string;
}

@Injectable()
export class AuthService {
  constructor(private http: HttpClient, private router: Router) {}

  currentUser: User | null = null;

  register(user: UserLogin) {
    return this.http.post('https://localhost:5001/api/Accounts/register', user);
  }

  onAutoLogin() {
    const userData: {
      username: string;
      token: string;
    } = JSON.parse(localStorage.getItem('user')!);
    if (!userData) {
      return;
    }
    const loadedUser: User = {
      username: userData.username,
      token: userData.token,
    };

    this.currentUser = loadedUser;
    this.userChanged.next(this.currentUser);
  }

  userChanged = new Subject<User | null>();

  login(user: UserLogin) {
    return this.http
      .post<User>('https://localhost:5001/api/Accounts/login', user)
      .pipe(delay(1500))

      .pipe(
        tap((resData) => {
          this.currentUser = {
            username: resData.username,
            token: resData.token,
          };
          this.userChanged.next(this.currentUser);
          localStorage.setItem('user', JSON.stringify(this.currentUser));
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser = null;
    this.userChanged.next(null);
    this.router.navigate(['']);
  }
}
