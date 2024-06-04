import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/Users`;

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

  updatePassword(
    userId: number,
    oldPassword: string,
    newPassword: string
  ): Observable<any> {
    const body = { OldPassword: oldPassword, NewPassword: newPassword };
    return this.http.put(`${this.apiUrl}/${userId}/update-password`, body, {
      responseType: 'text',
    });
  }

  getUserProfileImage(userId: number): Observable<Blob> {
    const url = `${this.apiUrl}/${userId}/profileImage`;

    // Specify responseType as 'blob' to handle binary data
    return this.http.get(url, { responseType: 'blob' });
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${userId}`);
  }
}
