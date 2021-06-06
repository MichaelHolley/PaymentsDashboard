import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tag } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  URL = "api/Statistics";

  constructor(private httpClient: HttpClient) { }

  getStackedBarChartByMonths(): Observable<any> {
    return this.httpClient.get<any[]>(this.URL + '/GetStackedBarChartByMonths');
  }

  getMonthlyAverageByTag(): Observable<any[]> {
    return this.httpClient.get<any[]>(this.URL + '/GetMonthlyAverageByTag');
  }
}
