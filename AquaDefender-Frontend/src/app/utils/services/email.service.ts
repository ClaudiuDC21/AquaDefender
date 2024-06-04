import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { EmailDto } from '../models/email-dto.model';


@Injectable({
  providedIn: 'root'
})
export class EmailService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/Email`;

  constructor(private http: HttpClient) {}

  sendEmail(emailDto: EmailDto): Observable<any> {
    return this.http.post<any>(this.apiUrl, emailDto);
  }
}
