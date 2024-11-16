import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MeterReadingService {
  private readonly apiUrl = 'http://localhost:5090/MeterReading';
  constructor(private http: HttpClient) {}

  uploadMeterReadings(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(`${this.apiUrl}/meter-reading-uploads`, formData);
  }
}
