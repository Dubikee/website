import LoginView from "../views/Login/Login.view";
import Home from "../views/Home/Home";
import Account from "../views/Account/Account.";
import { SwitchConfig } from "../utils";

export let index: SwitchConfig = {
	routes: [
		{
			path: '/login',
			component: LoginView
		},
		{
			path: '/home',
			component: Home
		},
		{
			path: '/account',
			component: Account
		}
	],
	redirects: [
		{
			from: '/',
			to: '/home/index'
		}
	]
}
