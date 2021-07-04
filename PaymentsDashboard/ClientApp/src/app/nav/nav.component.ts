import { Component, OnInit } from '@angular/core';
import { faChartLine, faHistory, faMoneyBillWave, faTag } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html'
})
export class NavComponent implements OnInit {
  faTag = faTag;
  faMoneyBillWave = faMoneyBillWave;
  faChartLine = faChartLine;
  faHistory = faHistory

  expandedOnMobile: boolean = false;

  routes: { route: string, title: string, icon: any }[] = [];

  constructor() { }

  ngOnInit() {
    this.routes.push({ route: '', title: 'Payments', icon: faMoneyBillWave });
    this.routes.push({ route: 'ReoccuringPayments', title: 'Reoccuring', icon: faHistory });
    this.routes.push({ route: 'Tags', title: 'Tags', icon: faTag });
    this.routes.push({ route: 'Charts', title: 'Charts', icon: faChartLine });
  }

  toggleNavbar(value: boolean) {
    this.expandedOnMobile = value;
  }
}
