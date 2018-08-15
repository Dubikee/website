import { RouteProps, RedirectProps } from "react-router";
import LoginView from "../views/Login/Login.view";
import TestView from "../views/Test/Test.view";
import IndexView from "../views/Index/Index.view";

export let routes: RouteProps[] = [
	{
		path: '/',
		exact: true,
		component: IndexView
	},
	{
		path: '/login',
		component: LoginView
	},
	{
		path: '/index',
		component: IndexView
	},
	{
		path: '/test/:id',
		component: TestView
	}
]

export let redirect: RedirectProps[] = [
	{
		from: '/test',
		to: '/home'
	},
	{
		to: '/login'
	}
]
