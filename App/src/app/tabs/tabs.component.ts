import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabs',
  templateUrl: './tabs.component.html',
  styleUrls: ['./tabs.component.css']
})
export class TabsComponent {
	tab = 'CompanySettings';

	constructor(private router: Router) {}

	navigate(page: string) {
		this.tab = page;
	}

	logout() {
		window.sessionStorage.removeItem('access_token');
		this.router.navigate(['/login']);
	}
}
