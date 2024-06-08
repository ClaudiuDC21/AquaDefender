import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/Locations`;

  constructor(private http: HttpClient) {}

  getAllCities(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/cities`);
  }

  getAllCounties(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/counties`);
  }

  getCityById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/cities/id/${id}`);
  }

  getCountyById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/counties/id/${id}`);
  }

  getAllCitiesByCountyId(countyId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/counties/${countyId}/cities`);
  }

  getCityByName(cityName: string): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}/cities/${encodeURIComponent(cityName)}`
    );
  }

  getCountyByName(countyName: string): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}/counties/${encodeURIComponent(countyName)}`
    );
  }

  getCityEmailById(cityId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/city/${cityId}/email`);
  }

  getCountyEmailById(countyId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/county/${countyId}/email`);
  }
}
