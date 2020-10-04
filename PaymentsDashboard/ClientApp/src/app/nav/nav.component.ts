import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html'
})
export class NavComponent implements OnInit {

  expandedOnMobile: boolean = false;

  constructor( ) { }

  ngOnInit() {
  }

  toggleNavbar(value: boolean) {
    this.expandedOnMobile = value;
  }
}
