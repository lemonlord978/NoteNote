import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

interface LoginResponse {
  message: string;
  userId: number;
}

interface RegisterResponse {
  message: string;
  userId: number;
}

interface getUser {
  username: string;
}

@Injectable({
  providedIn: 'root'
})

export class UserApiService {

  private apiUrl = 'http://localhost:5078/api/UserApi';

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<LoginResponse> {
    const loginData = { username, password };
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginData);
  }

  register(username: string, password: string, email: string): Observable<RegisterResponse> {
    const registerData = { username, password, email };
    return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, registerData);
  }

  getUserById(userId: number): Observable<getUser> {
    return this.http.post<getUser>(`${this.apiUrl}/getUserById/`, userId);
  }
}
