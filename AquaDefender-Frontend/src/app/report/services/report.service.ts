import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReportStatistics } from '../models/report-statistics.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/Report`;

  constructor(private http: HttpClient) {}

  getReportById(reportId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${reportId}`);
  }

  getImagesByReportId(reportId: number): Observable<Blob> {
    const url = `${this.apiUrl}/${reportId}/images`;

    return this.http.get(url, { responseType: 'blob' });
  }

  checkIfReportHasImages(reportId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/${reportId}/hasimages`);
  }

  getTotalReportsByUserId(userId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/User/${userId}/Total`);
  }

  getNewReportsByUserId(userId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/User/${userId}/New`);
  }

  getInProgressReportsByUserId(userId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/User/${userId}/InProgress`);
  }

  getResolvedReportsByUserId(userId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/User/${userId}/Resolved`);
  }

  getNewReportsByCityId(cityId: number): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/City/${cityId}/New`);
  }

  loadStatistics(cityId: number): Observable<ReportStatistics> {
    return this.http.get<ReportStatistics>(`${this.apiUrl}/stats/${cityId}`);
  }

  updateReportStatus(reportId: number, newStatus: number): Observable<any> {
    const params = new HttpParams().set('status', newStatus.toString());

    return this.http.put<any>(`${this.apiUrl}/${reportId}/status`, null, {
      params,
    });
  }

  getFilteredReportsbyUserId(
    userId: number,
    status?: string,
    severity?: string
  ): Observable<any[]> {
    let params = new HttpParams();

    params = params.append('userId', userId.toString());

    if (status && status !== 'Toate') {
      params = params.append('status', status);
    }
    if (severity && severity !== 'Toate') {
      params = params.append('severity', severity);
    }

    return this.http.get<any[]>(`${this.apiUrl}/user/status/severity`, {
      params,
    });
  }

  getFilteredReportsByCityId(
    cityId: number,
    status?: string,
    severity?: string,
    startDate?: string,
    endDate?: string,
    userName?: string
  ): Observable<any[]> {
    let params = new HttpParams();

    params = params.append('cityId', cityId.toString());

    if (status && status !== 'Toate') {
      params = params.append('status', status);
    }
    if (severity && severity !== 'Toate') {
      params = params.append('severity', severity);
    }

    if (startDate) {
      const start = new Date(startDate);
      if (!isNaN(start.getTime())) {
        params = params.append('startDate', start.toISOString().split('T')[0]);
      }
    }

    if (endDate) {
      const end = new Date(endDate);
      if (!isNaN(end.getTime())) {
        params = params.append('endDate', end.toISOString().split('T')[0]);
      }
    }

    if (userName) {
      params = params.append('userName', userName);
    }

    return this.http.get<any[]>(`${this.apiUrl}/filteredReportsByCityId`, {
      params,
    });
  }

  getRandomNewReports(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/random-new`);
  }

  getRandomInProgressReports(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/random-in-progress`);
  }

  getRandomCompletedReports(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/random-completed`);
  }

  createReportWithImages(reportDto: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, reportDto);
  }
}
