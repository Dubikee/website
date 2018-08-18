import { RouteProps, RedirectProps } from "react-router";
import CoursesView from "../views/Home/Courses/Courses.view";
import ScoresView from "../views/Home/Scores/Scores.view";

export let homeRoutes: RouteProps[] = [
	{
		path: '/home/index',
		component: CoursesView
	},
	{
		path: '/home/courses',
		component: CoursesView
	},
	{
		path: '/home/scores',
		component: ScoresView
	}
]

export let homeRedirect: RedirectProps[] = [
	{
		from: '/home',
		to: '/home/index'
	}
]
