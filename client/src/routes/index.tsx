import { RouteProps, RedirectProps } from "react-router";
import HomeView from "../views/Home/Home.view";
import TestView from "../views/Test/Test.view";

export let routes: RouteProps[] = [
    {
        path: '/',
        exact: true,
        component: HomeView
    },
    {
        path: '/home',
        component: HomeView
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
        to: '/home'
    }
]
