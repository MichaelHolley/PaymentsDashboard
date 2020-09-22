import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Payment, PaymentPostModel } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  URL = "api/Payment";

  constructor(private httpClient: HttpClient) { }

  getAllPayments(): Observable<Payment[]> {
    return this.httpClient.get<Payment[]>(this.URL);
  }

  createOrUpdatePayment(payment: PaymentPostModel): Observable<any> {
    return this.httpClient.post<PaymentPostModel>(this.URL, payment);
  }

  deletePayment(id: string): Observable<Payment> {
    return this.httpClient.delete<Payment>(this.URL + '/' + id);
  }
}
