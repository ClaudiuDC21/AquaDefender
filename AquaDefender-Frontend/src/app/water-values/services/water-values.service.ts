import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class WaterValuesService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/WaterValues`;

  constructor(private http: HttpClient) {}

  getWaterValuesById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  getAllWaterValuesByWaterInfoId(waterInfoId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/waterinfo/${waterInfoId}`);
  }

  getAllWaterValues(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }

  createWaterValues(waterValuesDto: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.post<any>(`${this.apiUrl}`, waterValuesDto, { headers });
  }

  updateWaterValues(id: number, waterValuesDto: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.put<any>(`${this.apiUrl}/${id}`, waterValuesDto, {
      headers,
    });
  }

  deleteWaterValues(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
