import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { CompanySettingsComponent } from './company-settings/company-settings.component';
import { TabsComponent } from './tabs/tabs.component';
import { NewUserComponent } from './new-user/new-user.component';

@NgModule({
	declarations: [
		AppComponent,
		LoginComponent,
		CompanySettingsComponent,
    	TabsComponent,
     NewUserComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		HttpClientModule,
		FormsModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule {}
