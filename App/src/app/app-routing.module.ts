import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { CompanySettingsComponent } from './company-settings/company-settings.component';
import { TabsComponent } from './tabs/tabs.component';
import { NewUserComponent } from './new-user/new-user.component';

const routes: Routes = [
	{ path: '', redirectTo: 'login', pathMatch: 'full' },
	{ path: 'login', component: LoginComponent },
	{ path: 'tabs', component: TabsComponent },
	{ path: 'company-settings', component: CompanySettingsComponent },
	{ path: 'new-user', component: NewUserComponent },
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule { }