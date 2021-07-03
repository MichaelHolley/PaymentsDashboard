import { Component, OnInit } from '@angular/core';
import { ReoccuringPaymentService } from '../../assets/shared/services/reoccuringpayment.service';

@Component({
  selector: 'app-reoccuring-payments',
  templateUrl: './reoccuring-payments.component.html'
})
export class ReoccuringPaymentsComponent implements OnInit {

  constructor(private reoccuringPaymentsService: ReoccuringPaymentService) { }

  ngOnInit() {
  }

}
