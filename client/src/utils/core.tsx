import registerServiceWorker from "./registerServiceWorker";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { RouteProps, RedirectProps, Switch, Route, Redirect } from "react-router";
import { BrowserRouter } from "react-router-dom";
import Axios, { AxiosRequestConfig } from 'axios';
import * as qs from 'querystring';
import { User } from "../common/User";

export let request = (url: string) => new HttpClient(url)
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
export let isMaster = (user: User) =>
	user && user.role == 'master';

export /**
 * 简化路由配置
 *
 * @param {RouteProps[]} routes
 * @param {RedirectProps[]} [redirect]
 * @returns
 */
	let renderRouter = (routes: RouteProps[], redirect?: RedirectProps[]) => {
		return (
			<BrowserRouter basename="/">
				<Switch>
					{routes.map((x, i) => <Route key={i} {...x} />)}
					{redirect ? redirect.map((x, i) => <Redirect key={i} {...x} />) : null}
				</Switch>
			</BrowserRouter>
		);
	};

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


/**
 * 发送Http请求的类
 *
 * @export
 * @class HttpClient
 */
export class HttpClient {
	private headers: any;
	private data: any;
	constructor(private url: string) {
		this.url = url;
	}
	/**
	 * 添加头
	 *
	 * @param {string} key
	 * @param {string} value
	 * @memberof HttpClient
	 */
	public header(key: string, value: string) {
		this.headers = { [key]: value, ...this.headers }
		return this;

	}
	/**
	 * 添加表单数据
	 *
	 * @param {string} key
	 * @param {string} value
	 * @memberof HttpClient
	 */
	public form(key: string, value: string) {
		this.data = { [key]: value, ...this.data }
		return this;
	}
	/**
	 * 设置表单数据
	 *
	 * @param {*} data
	 * @memberof HttpClient
	 */
	public forms(data: any) {
		this.data = data
		return this;
	}
	/**
	 *	设置Jwt验证
	 *
	 * @param {string} jwt
	 * @memberof HttpClient
	 */
	public auth(jwt: string) {
		this.headers = { 'Authorization': 'Bearer ' + jwt, ...this.headers }
		return this;
	}
	/**
	 * 发起Get请求
	 *
	 * @template T
	 * @returns
	 * @memberof HttpClient
	 */
	public async get<T>() {
		let config: AxiosRequestConfig = {}
		if (this.headers)
			config.headers = this.headers;
		if (this.data)
			config.params = this.data;
		return Axios.get<T>(this.url, config)
	}
	/**
	 * 发起post请求
	 *
	 * @template T
	 * @returns
	 * @memberof HttpClient
	 */
	public async post<T>() {
		let config: AxiosRequestConfig = {}
		if (this.headers)
			config.headers = this.headers;
		return Axios.post<T>(this.url, qs.stringify(this.data), config)
	}
}
