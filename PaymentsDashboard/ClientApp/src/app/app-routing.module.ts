import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentsComponent } from './payments/payments.component';
import { ChartsComponent } from './charts/charts.component';
import { TagsComponent } from './tags/tags.component';

const routes: Routes = [
  { path: '', component: PaymentsComponent },
  { path: 'Payments', component: PaymentsComponent },
  { path: 'Tags', component: TagsComponent },
  { path: 'Charts', component: ChartsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
