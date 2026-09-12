import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

interface LoginResponse {
  token: string;
  expiration: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly authUrl = 'https://localhost:7015/api/Auth/login';
  private readonly tokenKey = 'assessmentJwtToken';
  private readonly loggedInSubject =
    new BehaviorSubject<boolean>(this.hasToken());

  public loggedIn$ =
    this.loggedInSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.authUrl, { username, password }).pipe(
      tap((response) => {
        localStorage.setItem(this.tokenKey, response.token);
        this.loggedInSubject.next(true);
      })
    );
  }

  getToken(): string {
    return localStorage.getItem(this.tokenKey) ?? '';
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.loggedInSubject.next(false);
  }

  hasToken(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }
}
