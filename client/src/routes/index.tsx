import { RouteProps, RedirectProps } from "react-router";
import LoginView from "../views/Login/Login.view";
import HomeLayout from "../views/Home/Layout/Home.layout";

export let indexRoutes: RouteProps[] = [
	{
		path: '/login',
		component: LoginView
	},
	{
		path: '/home',
		component: HomeLayout
	}
]

export let indexRedirect: RedirectProps[] = [
	{
		from: '/',
		to: '/home/index'
	}
]
