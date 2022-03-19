import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentsComponent } from './payments/payments.component';
import { ChartsComponent } from './charts/charts.component';
import { TagsComponent } from './tags/tags.component';
import { ReoccuringPaymentsComponent } from './reoccuring-payments/reoccuring-payments.component';
import { AuthGuard } from '@auth0/auth0-angular';

const routes: Routes = [
  { path: '', component: PaymentsComponent },
  { path: 'Payments', component: PaymentsComponent, canActivate: [AuthGuard] },
  { path: 'Tags', component: TagsComponent, canActivate: [AuthGuard] },
  { path: 'Charts', component: ChartsComponent, canActivate: [AuthGuard] },
  { path: 'ReoccuringPayments', component: ReoccuringPaymentsComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
