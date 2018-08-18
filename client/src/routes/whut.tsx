import { RouteProps, RedirectProps } from "react-router";
import CoursesView from "../views/Home/Courses/Courses.view";

export let whutRoutes: RouteProps[] = [
	{
		path: '/home/index',
		component: CoursesView
	},
	{
		path: '/home/courses',
		component: CoursesView
	}
]

export let whutRedirect: RedirectProps[] = [
	{
		from: '/home',
		to: '/home/index'
	}
]
