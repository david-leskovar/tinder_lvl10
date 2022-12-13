import { HttpClient } from '@angular/common/http';
import { createInjectableType } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginationHeaders } from '../helpers/paginationHelper';
import { getPaginatedResult } from '../helpers/paginationHelper';
import { Message } from '../models/message';

@Injectable()
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMessages(pageNumber: number, pageSize: number, container: any) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(
      this.baseUrl + 'messages',
      params,
      this.http
    );
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(
      this.baseUrl + 'messages/thread/' + username
    );
  }

  sendMessage(username: string, content: string) {
    return this.http.post<Message>(this.baseUrl + 'messages', {
      recipientUsername: username,
      content,
    });
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
