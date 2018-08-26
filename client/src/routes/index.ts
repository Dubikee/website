import LoginView from "../views/Login/Login.view";
import Home from "../views/Home/Home";
import Account from "../views/Account/Account.";
import CoursesView from "../views/Home/Courses/Courses.view";
import ScoresView from "../views/Home/Scores/Scores.view";
import RegisterView from '../views/Account/Register/Register.view'
import { SwitchConfig, renderSwitch } from "../utils";

export let index: SwitchConfig = {
	routes: [{
		path: '/login',
		component: LoginView
	}, {
		path: '/home',
		component: Home
	}, {
		path: '/account',
		component: Account
	}],
	redirects: [{
		from: '/',
		to: '/home/index'
	}]
}


export let home: SwitchConfig = {
	routes: [{
		path: '/home/index',
		component: CoursesView
	}, {
		path: '/home/courses',
		component: CoursesView
	}, {
		path: '/home/scores',
		component: ScoresView
	}],
	redirects: [{
		from: '/home',
		to: '/home/index'
	}]
}


export let account: SwitchConfig = {
	routes: [{
		path: '/account/index',
		component: RegisterView
	},],
	redirects: [{
		from: '/account',
		to: '/acconut/index'
	}]
}