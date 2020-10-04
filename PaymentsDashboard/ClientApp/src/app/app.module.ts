import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';

import { PaymentService } from '../assets/shared/services/payment.service';
import { TagService } from '../assets/shared/services/tag.service';

import { AppComponent } from './app.component';
import { PaymentsComponent } from './payments/payments.component';
import { TagsComponent } from './tags/tags.component';
import { NavComponent } from './nav/nav.component';

@NgModule({
  declarations: [
    AppComponent,
    PaymentsComponent,
    TagsComponent,
    NavComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [
    PaymentService,
    TagService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
