import { Component, OnInit } from '@angular/core';
import { faMoneyBillWave, faTag } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html'
})
export class NavComponent implements OnInit {
  faTag = faTag;
  faMoneyBillWave = faMoneyBillWave;

  expandedOnMobile: boolean = false;

  constructor( ) { }

  ngOnInit() {
  }

  toggleNavbar(value: boolean) {
    this.expandedOnMobile = value;
  }
}
