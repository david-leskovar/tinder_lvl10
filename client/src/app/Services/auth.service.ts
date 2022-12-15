import { HttpClient } from '@angular/common/http';
import { Token } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, delay, Subject, tap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { MembersService } from './members.service';
import { PresenceService } from './presence.service';

export interface UserLogin {
  username: string;
  password: string;
}

export interface UserRegister {
  username: string;
  password: string;
  knownAs: string;
  gender: string;
  dateOfBirth: Date;
  city: string;
  country: string;
}

export interface User {
  username: string;
  token: string;
  photoURL?: string;
  knownAs?: string;
  gender?: string;
  roles: string[];
}

@Injectable()
export class AuthService {
  constructor(
    private http: HttpClient,
    private router: Router,
    private membersService: MembersService,
    private presence: PresenceService
  ) {}

  baseUrl = environment.apiUrl;

  currentUser: User | null = null;
  currentMember: Member | null = null;

  register(user: UserRegister) {
    return this.http.post(this.baseUrl + 'accounts/register', user);
  }

  onAutoLogin() {
    const userData: {
      username: string;
      token: string;
      gender: string;
    } = JSON.parse(localStorage.getItem('user')!);
    if (!userData) {
      return;
    }

    let userRoles = [];
    const roles = this.getDecodedToken(userData.token).role;
    Array.isArray(roles) ? (userRoles = roles) : userRoles.push(roles);

    const loadedUser: User = {
      username: userData.username,
      token: userData.token,
      gender: userData.gender,
      roles: userRoles,
    };

    this.currentUser = loadedUser;
    this.userChanged.next(this.currentUser);
  }

  userChanged = new Subject<User | null>();

  login(user: UserLogin) {
    return this.http.post<User>(this.baseUrl + 'Accounts/login', user).pipe(
      tap((resData) => {
        let userRoles = [];
        const roles = this.getDecodedToken(resData.token).role;
        Array.isArray(roles) ? (userRoles = roles) : userRoles.push(roles);

        this.currentUser = {
          username: resData.username,
          token: resData.token,
          gender: resData.gender,
          roles: userRoles,
        };
        this.userChanged.next(this.currentUser);

        this.presence.createHubConnection(this.currentUser);

        localStorage.setItem('user', JSON.stringify(this.currentUser));
      })
    );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser = null;
    this.userChanged.next(null);
    this.router.navigate(['/register']);
    this.presence.stopHubConnection();
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
