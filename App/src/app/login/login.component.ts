import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';

import { EmailVerification } from './email-verification';
import { LoginVerification } from './login-verification';
import { AccessTokenVerification } from './access-token-verification';

const ACCESS_TOKEN = 'access_token';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
	email = '';
	password = '';
	badAttemptMessage = '';
	isEmailVerified = false;
	verifying = false;

	constructor(private http: HttpClient, private router: Router) {}

	ngOnInit() {
		this.email = '';
		this.password = '';
		this.badAttemptMessage = '';
		this.isEmailVerified = false;
		this.verifying = false;
		
		this.verifyAccessToken();
	}

	async verifyAccessToken() {
		let accessToken = window.sessionStorage.getItem(ACCESS_TOKEN);

		if (!accessToken) {
			window.sessionStorage.removeItem(ACCESS_TOKEN);
			return;
		}

		const params = new HttpParams().set('accessToken', accessToken);

		this.http.get<AccessTokenVerification>('http://localhost:5000/api/verifyAccessToken',
			{ params: params, observe: 'response' }).subscribe(
			(resp) => {
				if (resp.ok) {
					if (resp.body?.isAccessTokenVerified === true) {
						this.router.navigate(['/tabs']);
					} else if (resp.body?.isAccessTokenVerified === false) {
						window.sessionStorage.removeItem(ACCESS_TOKEN);
					}
				}
			}, (err) => {
				alert(err.message);
		});
	}

	verifyEmail() {
		if (this.email.length == 0) {
			this.badAttemptMessage = 'Email cannot be empty';
			return;
		}

		this.verifying = true;
		this.badAttemptMessage = '';

		const params = new HttpParams().set('email', this.email);

		this.http.get<EmailVerification>('http://localhost:5000/api/verifyEmail',
			{ params: params, observe: 'response' }).subscribe(
			(resp) => {
				if (resp.ok) {
					if (resp.body?.isEmailVerified && typeof resp.body?.isEmailVerified === 'boolean') {
						this.isEmailVerified = resp.body?.isEmailVerified;
					} else {
						this.isEmailVerified = false;
					}

					if (this.isEmailVerified === false) {
						this.badAttemptMessage = 'Unrecognized email';
					}
				}

				this.verifying = false;

			}, (err) => {
				alert(err.message);

				this.verifying = false;
		});
	}

	back() {
		this.password = '';
		this.badAttemptMessage = '';
		this.isEmailVerified = false;
	}

	login() {
		if (this.password.length == 0) {
			this.badAttemptMessage = 'Password cannot be empty';
			return;
		}

		this.verifying = true;

		const params = new HttpParams()
			.set('email', this.email)
			.set('password', this.password);

		this.http.get<LoginVerification>('http://localhost:5000/api/verifyLogin',
			{ params: params, observe: 'response' }).subscribe(
			(resp) => {
				if (resp.ok) {
					if (resp.body?.accessToken && typeof resp.body?.accessToken === 'string') {
						window.sessionStorage.setItem(ACCESS_TOKEN, resp.body?.accessToken);
						this.router.navigate(['/tabs']);
					} else {
						this.badAttemptMessage = 'Incorrect login';
						window.sessionStorage.removeItem(ACCESS_TOKEN);
					}
				} else {
					this.badAttemptMessage = 'Incorrect login';
				}

				this.verifying = false;
			}, (err) => {
				alert(err.message);

				this.verifying = false;
		});
	}
}
