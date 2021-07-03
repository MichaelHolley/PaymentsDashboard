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

  constructor( ) { }

  ngOnInit() {
  }

  toggleNavbar(value: boolean) {
    this.expandedOnMobile = value;
  }
}
