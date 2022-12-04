import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-errors',
  templateUrl: './errors.component.html',
  styleUrls: ['./errors.component.css'],
})
export class ErrorsComponent implements OnInit {
  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  onError1() {
    this.http.get('http://localhost:5000/api/Buggy/auth').subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onError2() {
    this.http.get('http://localhost:5000/api/Buggy/not-found').subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onError3() {
    this.http.get('http://localhost:5000/api/Buggy/server-error').subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  onError4() {
    this.http.get('http://localhost:5000/api/Buggy/bad-request').subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
