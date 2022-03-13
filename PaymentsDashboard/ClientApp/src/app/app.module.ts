import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgApexchartsModule } from 'ng-apexcharts';

import { PaymentService } from './shared/services/payment.service';
import { TagService } from './shared/services/tag.service';

import { AppComponent } from './app.component';
import { PaymentsComponent } from './payments/payments.component';
import { TagsComponent } from './tags/tags.component';
import { NavComponent } from './nav/nav.component';
import { ChartsComponent } from './charts/charts.component';
import { ChartsService } from './shared/services/charts.service';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from './shared/dialogs/confirm-dialog.component';
import { InputValidationComponent } from './shared/inputcomponents/input-validation.component';
import { ReoccuringPaymentsComponent } from './reoccuring-payments/reoccuring-payments.component';
import { ReoccuringPaymentService } from './shared/services/reoccuringpayment.service';
import { DateTimeHelperService } from './shared/services/datetimehelper.service';
import { AuthModule } from '@auth0/auth0-angular';

@NgModule({
  declarations: [
    AppComponent,
    PaymentsComponent,
    TagsComponent,
    NavComponent,
    ChartsComponent,
    ConfirmDialogComponent,
    InputValidationComponent,
    ReoccuringPaymentsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AuthModule.forRoot({
      domain: '',
      clientId: ''
    }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    FontAwesomeModule,
    NgApexchartsModule,
    ModalModule.forRoot()
  ],
  entryComponents: [
    ConfirmDialogComponent
  ],
  providers: [
    PaymentService,
    TagService,
    ChartsService,
    ReoccuringPaymentService,
    DateTimeHelperService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
