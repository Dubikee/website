import RegisterView from "../views/Account/Register/Register.view";
import { SwitchConfig } from "../utils";

export let account: SwitchConfig = {
	routes: [
		{
			path: '/account/index',
			component: RegisterView
		},
	],
	redirects: [{
		from: '/account',
		to: '/acconut/index'
	}]
}
