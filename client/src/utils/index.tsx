import registerServiceWorker from "./registerServiceWorker";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { RouteProps, RedirectProps, Switch, Route, Redirect } from "react-router";
import { User } from "../common/stores/User";

export let bootstrap = () => new Bootstrap();
export type nullable = null | undefined
export interface IBootstrap {
	with(App: React.ComponentClass): IBootstrap;
	does(work: () => Promise<void> | void): IBootstrap;
	mount(selector: string): IBootstrap;
	start(): void;
};
export let getToken = () => window.localStorage.getItem("jwt");
export let setToken = (jwt: string) => window.localStorage.setItem("jwt", jwt)
export let removeToken = () => window.localStorage.removeItem('jwt')
export let isMaster = (user: User) => user && user.role == 'master';
export let isAdmin = (user: User | nullable) => user && (user.role == 'master' || user.role == 'admin');
export let renderRouter = (routes: RouteProps[], redirect?: RedirectProps[]) => <Switch>
	{routes.map((x, i) => <Route key={i} {...x} />)}
	{redirect ? redirect.map((x, i) => <Redirect key={i} {...x} />) : null}
</Switch>;

export let match = (switcher: any) => (v: string | number) => {
	if (switcher[v])
		switcher[v]()
	else if (switcher['_'])
		switcher['_']()
	if (switcher['*'])
		switcher['*']()
}

class Bootstrap implements IBootstrap {

	private works: Array<() => Promise<void> | void> = [registerServiceWorker];
	private App: React.ComponentClass;
	private ele: Element;


	/**
	 * 设置启动根组件
	 * @param {React.ComponentClass} App
	 * @returns
	 * @memberof Bootstrap
	 */
	public with(App: React.ComponentClass) {
		this.App = App;
		return this;
	}


	/**
	 * 添加启动时的任务，有顺序，默认为registerServiceWorker
	 * @param {(() => Promise<void> | void)} work
	 * @returns
	 * @memberof Bootstrap
	 */
	public does(work: () => Promise<void> | void) {
		this.works.push(work);
		return this;
	}


	/**
	 * 设置启动挂载点
	 * @param {string} selector
	 * @returns
	 * @memberof Bootstrap
	 */
	public mount(selector: string) {
		let e = document.querySelector(selector);
		if (e) this.ele = e;
		else throw Error("找不到挂载点");
		return this;
	}

	public start() {
		if (!this.ele || !this.App) throw Error("根组件与挂载点不可为空");
		let { works, App, ele } = this;
		ReactDOM.render(<App />, ele);
		works.forEach(f => f());
	}
}


