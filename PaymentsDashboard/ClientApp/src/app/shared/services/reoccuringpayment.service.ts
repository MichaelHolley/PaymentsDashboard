import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReoccuringPayment } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class ReoccuringPaymentService {

  URL = "api/ReoccuringPayment";

  constructor(private httpClient: HttpClient) { }

  getAllReoccuringPayments(): Observable<ReoccuringPayment[]> {
    return this.httpClient.get<ReoccuringPayment[]>(this.URL);
  }

  createOrUpdateReoccuringPayment(payment: ReoccuringPayment): Observable<any> {
    return this.httpClient.post<ReoccuringPayment>(this.URL, payment);
  }

  deleteReoccuringPayment(id: string): Observable<ReoccuringPayment> {
    return this.httpClient.delete<ReoccuringPayment>(this.URL + '/' + id);
  }
}
