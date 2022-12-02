import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { AuthService } from './Services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit() {
    this.authService.onAutoLogin();
  }

  title = 'client';
}
