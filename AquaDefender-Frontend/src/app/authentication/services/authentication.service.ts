import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { UserRole } from '../enums/user-role';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/Authentication`;
  private tokenKey = 'auth_token';
  private emailKey = 'auth_email';
  private userIDKey = 'user_id';
  private cityIDKey = 'auth_city_id';
  private userNameKey = 'auth_user_name';
  private roleKey = 'auth_role_id';

  constructor(private http: HttpClient) {}

  register(user: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.post<any>(`${this.apiUrl}/register`, user, { headers });
  }

  login(user: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, user).pipe(
      tap(
        (response) => this.handleLoginAuthentication(response),
        (error) => console.error('Login error:', error)
      )
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.emailKey);
    localStorage.removeItem(this.userIDKey);
    localStorage.removeItem(this.cityIDKey);
    localStorage.removeItem(this.userNameKey);
    localStorage.removeItem(this.roleKey);
  }

  private handleLoginAuthentication(response: any): void {
    console.log('Authentication response:', response);

    if (response && response.token) {
      localStorage.setItem(this.tokenKey, response.token);
    }

    if (response && response.email) {
      localStorage.setItem(this.emailKey, response.email);
    }

    if (response && response.userId) {
      localStorage.setItem(this.userIDKey, response.userId.toString());
    }

    if (response && response.cityId) {
      localStorage.setItem(this.cityIDKey, response.cityId.toString());
    }

    if (response && response.userName) {
      localStorage.setItem(this.userNameKey, response.userName);
    }

    if (response && response.roleId) {
      localStorage.setItem(this.roleKey, response.roleId.toString());
    }
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getUserId(): number | null {
    const userIdString = localStorage.getItem(this.userIDKey);

    if (userIdString === null || userIdString === undefined) {
      return null;
    }

    const userId = parseInt(userIdString, 10);

    if (isNaN(userId)) {
      return null;
    }

    return userId;
  }

  getEmail(): string | null {
    return localStorage.getItem(this.emailKey);
  }

  getUserName(): string | null {
    return localStorage.getItem(this.userNameKey);
  }

  getAuthStatus(): boolean {
    const token = localStorage.getItem(this.tokenKey);
    return !!token;
  }

  getCityId(): string | null {
    return localStorage.getItem(this.cityIDKey);
  }

  getUserRole(): UserRole | null {
    const roleString = localStorage.getItem(this.roleKey);

    if (roleString === null || roleString === undefined) {
      return null;
    }

    const role = parseInt(roleString, 10);

    if (isNaN(role)) {
      return null;
    }

    return role as UserRole;
  }

  getUserRoleId(): string | null {
    const userRole = this.getUserRole();

    if (userRole === null) {
      return null;
    }

    return userRole.toString();
  }

  setUserName(userName: string): void {
    localStorage.setItem(this.userNameKey, userName);
  }

  setCityId(cityId: string): void {
    localStorage.setItem(this.cityIDKey, cityId);
  }

  removeUserName(): void {
    localStorage.removeItem(this.userNameKey);
  }

  removeCityId(): void {
    localStorage.removeItem(this.cityIDKey);
  }

  isAdmin(): boolean {
    return this.getUserRole() === UserRole.Admin;
  }

  isUser(): boolean {
    return this.getUserRole() === UserRole.User;
  }

  isCityHallEmployee(): boolean {
    return this.getUserRole() === UserRole.CityHallEmployee;
  }

  isWaterDepartmentEmployee(): boolean {
    return this.getUserRole() === UserRole.WaterDepartmentEmployee;
  }

  isNotUser(): boolean {
    const role = this.getUserRole();
    return (
      role === UserRole.Admin ||
      role === UserRole.CityHallEmployee ||
      role === UserRole.WaterDepartmentEmployee
    );
  }
}
