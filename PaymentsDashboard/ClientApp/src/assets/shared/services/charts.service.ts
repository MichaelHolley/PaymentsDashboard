import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChartsService {

  URL = "api/Charts";

  constructor(private httpClient: HttpClient) { }

  getStackedBarChartByMonths(): Observable<any> {
    return this.httpClient.get<any[]>(this.URL + '/GetStackedBarChartByMonths');
  }

  getMonthlyAverageByTag(): Observable<any[]> {
    return this.httpClient.get<any[]>(this.URL + '/GetMonthlyAverageByTag');
  }
}
