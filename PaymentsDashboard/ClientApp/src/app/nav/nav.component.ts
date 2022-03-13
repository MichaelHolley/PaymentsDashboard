import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { faChartLine, faHistory, faMoneyBillWave, faSignInAlt, faSignOutAlt, faTag } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html'
})
export class NavComponent implements OnInit {
  faTag = faTag;
  faMoneyBillWave = faMoneyBillWave;
  faChartLine = faChartLine;
  faHistory = faHistory;
  faSignIn = faSignInAlt;
  faSignOut = faSignOutAlt;

  expandedOnMobile: boolean = false;

  routes: { route: string, title: string, icon: any }[] = [];

  constructor(public auth: AuthService) {
    auth.user$.subscribe(p => console.log(p));
    auth.idTokenClaims$.subscribe(p => console.log(p));
    auth.appState$.subscribe(p => console.log(p));
    auth.getAccessTokenSilently().subscribe(p => console.log(p));
  }

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
