import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgApexchartsModule } from 'ng-apexcharts';
import { ModalModule } from 'ngx-bootstrap/modal';
import { env } from 'process';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ChartsComponent } from './charts/charts.component';
import { NavComponent } from './nav/nav.component';
import { PaymentsComponent } from './payments/payments.component';
import { ReoccuringPaymentsComponent } from './reoccuring-payments/reoccuring-payments.component';
import { ConfirmDialogComponent } from './shared/dialogs/confirm-dialog.component';
import { InputValidationComponent } from './shared/inputcomponents/input-validation.component';
import { ChartsService } from './shared/services/charts.service';
import { DateTimeHelperService } from './shared/services/datetimehelper.service';
import { PaymentService } from './shared/services/payment.service';
import { ReoccuringPaymentService } from './shared/services/reoccuringpayment.service';
import { TagService } from './shared/services/tag.service';
import { TagsComponent } from './tags/tags.component';


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
    HttpClientModule,
    AuthModule.forRoot({
      // The domain and clientId were configured in the previous chapter
      domain: '',
      clientId: '',
      audience: 'https://localhost:44347/api',           
      httpInterceptor: {
        allowedList: [
          "*"
        ]
      }
    }),
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
    DateTimeHelperService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
