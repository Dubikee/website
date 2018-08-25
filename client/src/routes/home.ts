import CoursesView from "../views/Home/Courses/Courses.view";
import ScoresView from "../views/Home/Scores/Scores.view";
import { SwitchConfig } from "../utils";

export let home: SwitchConfig = {
	routes: [
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
	],
	redirects: [
		{
			from: '/home',
			to: '/home/index'
		}
	]
}
