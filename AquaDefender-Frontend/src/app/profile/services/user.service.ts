import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'https://localhost:2112/Users'; // assuming the UsersController route

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}`);
  }

  getUserById(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${userId}`);
  }

  updateUser(userId: number, userDto: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${userId}`, userDto);
  }

  updatePassword(userId: number, oldPassword: string, newPassword: string): Observable<any> {
    const body = { OldPassword: oldPassword, NewPassword: newPassword };
    return this.http.put(`${this.apiUrl}/${userId}/update-password`, body, {
      responseType: 'text'
    });
  }
  
  

  deleteUser(userId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${userId}`);
  }
}
