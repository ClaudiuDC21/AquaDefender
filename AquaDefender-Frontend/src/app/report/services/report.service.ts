// report.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReportStatistics } from '../models/report-statistics.model';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  private apiUrl = 'https://localhost:2112/Report'; // assuming the ReportController route

  constructor(private http: HttpClient) {}

  getReportById(reportId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${reportId}`);
  }

  getAllReports(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }

  getAllImages(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/images`);
  }

  getImagesByReportId(reportId: number): Observable<Blob> {
    const url = `${this.apiUrl}/${reportId}/images`;

    // Specify responseType as 'blob' to handle binary data
    return this.http.get(url, { responseType: 'blob' });
  }

  checkIfReportHasImages(reportId: number): Observable<boolean> {
    // Verifică dacă un raport are imagini folosind un endpoint adecvat
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
    // Construiește parametrii URL-ului
    const params = new HttpParams().set('status', newStatus.toString());

    // Trimite cererea PUT cu parametrii în URL
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

    // Face request-ul GET cu parametrii construiți
    return this.http.get<any[]>(`${this.apiUrl}/user/status/severity`, {
      params,
    });
  }

  getFilteredReportsByCityId(
    cityId: number,
    status?: string,
    severity?: string,
    startDate?: string, // Expecting a date string from input[type=date] or a similar source
    endDate?: string, // Same as above
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

    // Validate and append startDate if present
    if (startDate) {
      const start = new Date(startDate);
      if (!isNaN(start.getTime())) {
        params = params.append('startDate', start.toISOString().split('T')[0]);
      }
    }

    // Validate and append endDate if present
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

  // Method to get randomly selected in-progress reports
  getRandomInProgressReports(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/random-in-progress`);
  }

  // Method to get randomly selected completed reports
  getRandomCompletedReports(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/random-completed`);
  }

  // getImagesByReportId(reportId: number): Observable<Blob> {
  //   const url = `${this.apiUrl}/${reportId}/images`;

  //   // Specify responseType as 'blob' to handle binary data
  //   return this.http.get(url, { responseType: 'blob'});
  // }

  createReportWithImages(reportDto: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, reportDto);
  }

  updateReport(reportId: number, reportDto: FormData): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${reportId}`, reportDto);
  }

  deleteReport(reportId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${reportId}`);
  }
}
