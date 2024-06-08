import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class WaterInfoService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/WaterInfo`;

  constructor(private http: HttpClient) {}

  getWaterInfoById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  getAllWaterInfos(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }

  getAllWaterInfosByCityId(cityId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/city/${cityId}`);
  }

  getReportByDateAndCity(date: string, cityId: number): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}/report?date=${date}&cityId=${cityId}`
    );
  }

  createWaterInfo(waterInfoDto: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.post<any>(`${this.apiUrl}`, waterInfoDto, { headers });
  }

  updateWaterInfo(id: number, waterInfoDto: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.put<any>(`${this.apiUrl}/${id}`, waterInfoDto, {
      headers,
    });
  }

  deleteWaterInfo(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
