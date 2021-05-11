import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentsComponent } from './payments/payments.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { TagsComponent } from './tags/tags.component';

const routes: Routes = [
  { path: '', component: PaymentsComponent },
  { path: 'Payments', component: PaymentsComponent },
  { path: 'Tags', component: TagsComponent },
  { path: 'Statistics', component: StatisticsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
