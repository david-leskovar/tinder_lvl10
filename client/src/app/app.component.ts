import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { AuthService } from './Services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { ThemePalette } from '@angular/material/core';
import { ProgressBarMode } from '@angular/material/progress-bar';
import { PresenceService } from './Services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private presence: PresenceService
  ) {}

  ngOnInit() {
    this.authService.onAutoLogin();

    if (this.authService.currentUser) {
      this.presence.createHubConnection(this.authService.currentUser);
    }
  }

  title = 'client';
}
