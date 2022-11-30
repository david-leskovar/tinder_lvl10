import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  constructor(private http: HttpClient) {}
  title = 'client';
  onFetch() {
    this.http
      .get('https://localhost:7195/api/UsersContoller')
      .subscribe((data) => {
        console.log(data);
      });
  }
}
