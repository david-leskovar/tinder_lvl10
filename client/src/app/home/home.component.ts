import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Member } from '../models/member';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  constructor(private http: HttpClient, public authService: AuthService) {}

  ngOnInit(): void {}
}
