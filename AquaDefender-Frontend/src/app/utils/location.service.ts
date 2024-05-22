import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private apiUrl = 'https://localhost:2112/Locations'; // assuming the LocationsController route

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
    return this.http.get<any>(`${this.apiUrl}/cities/${encodeURIComponent(cityName)}`);
  }

  getCountyByName(countyName: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/counties/${encodeURIComponent(countyName)}`);
  }
}
