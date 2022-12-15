import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.css'],
})
export class ErrorsComponent implements OnInit {
  baseUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  onError1() {
    this.http.get(this.baseUrl + 'Buggy/auth').subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onError2() {
    this.http.get(this.baseUrl + 'Buggy/not-found').subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onError3() {
    this.http.get(this.baseUrl + 'Buggy/server-error').subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onError4() {
    this.http.get(this.baseUrl + 'Buggy/bad-request').subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
