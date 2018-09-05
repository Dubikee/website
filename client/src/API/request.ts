import Axios, { AxiosRequestConfig } from "axios";
import * as qs from "querystring";
import { API } from ".";
import { Status } from "./Status";
import { BaseModel } from "./models/BaseModel";
import { parseStatus } from "../utils";

export let request = <URL extends keyof API>(url: URL) =>
	new HttpClient<API[URL]>(url);

interface IResponseCallBack<T> {
	onOk: (data: T) => any;
	before?: () => any;
	onNotAllowed?: () => any;
	onTokenExpired?: () => any;
	onUidIllegal?: () => any;
	onUidNotFind?: () => any;
	onUidHasExist?: () => any;
	onPwdWrong?: () => any;
	onPwdIllegal?: () => any;
	onWhutPwdWrong?: () => any;
	onWhutIdNotFind?: () => any;
	onInputIllegal?: () => any;
	onWhutCrashed?: () => any;
	///请求错误
	on401Unauthorized?: () => any;
	on403Forbidden?: () => any;
	on423Locked?: () => any;
	///未处理的错误
	onUnknownError: () => any;
}

class HttpClient<T extends BaseModel> {
	private headers: any;
	private data: any;
	private callbacks: IResponseCallBack<T>;
	constructor(private url: string) { }
	public header(key: string, value: any) {
		if (this.headers) this.headers[key] = value;
		else this.headers = { [key]: value };
		return this;
	}
	public form(key: string, value: any) {
		if (this.data) this.data[key] = value;
		else this.data = { [key]: value };
		return this;
	}
	public forms(data: any) {
		this.data = data;
		return this;
	}
	public auth(jwt: string) {
		return this.header("Authorization", "Bearer " + jwt);
	}

	public with(callbacks: IResponseCallBack<T>) {
		this.callbacks = callbacks;
		return this;
	}

	public async get() {
		await this.request();
	}
	public async post() {
		await this.request("post");
	}

	private async request(method = "get") {
		const config: AxiosRequestConfig = {};
		if (this.headers) config.headers = this.headers;
		let runBefore = false;
		try {
			var data: T | null = null;
			if (method === "get") {
				if (this.data) config.params = this.data;
				const res = await Axios.get<T>(this.url, config);
				data = res.data;
			} else if (method == "post") {
				const res = await Axios.post<T>(
					this.url,
					qs.stringify(this.data),
					config
				);
				data = res.data;
			} else {
				return;
			}
			console.log(
				`${this.url} response ok with data status ${data.status}`
			);
			if (this.callbacks.before) {
				this.callbacks.before();
				runBefore = true;
			}
			switch (data.status) {
				case Status.Ok:
					if (this.callbacks.onOk) this.callbacks.onOk(data);
					break;
				case Status.NotAllowed:
					if (this.callbacks.onNotAllowed)
						this.callbacks.onNotAllowed();
					break;
				case Status.TokenExpired:
					if (this.callbacks.onTokenExpired)
						this.callbacks.onTokenExpired();
					break;
				case Status.UidIllegal:
					if (this.callbacks.onUidIllegal)
						this.callbacks.onUidIllegal();
					break;
				case Status.UidNotFind:
					if (this.callbacks.onUidNotFind)
						this.callbacks.onUidNotFind();
					break;
				case Status.UidHasExist:
					if (this.callbacks.onUidHasExist)
						this.callbacks.onUidHasExist();
					break;
				case Status.PwdWrong:
					if (this.callbacks.onPwdWrong) this.callbacks.onPwdWrong();
					break;
				case Status.PwdIllegal:
					if (this.callbacks.onPwdIllegal)
						this.callbacks.onPwdIllegal();
					break;
				case Status.WhutPwdWrong:
					if (this.callbacks.onWhutPwdWrong)
						this.callbacks.onWhutPwdWrong();
					break;
				case Status.WhutCrashed:
					if (this.callbacks.onWhutCrashed)
						this.callbacks.onWhutCrashed();
					break;
				case Status.InputIllegal:
					if (this.callbacks.onInputIllegal)
						this.callbacks.onInputIllegal();
					break;
			}
		} catch (error) {
			const code = parseStatus(error);
			console.log(`${this.url} response error with HttpStatusCode ${code}`);
			if (!runBefore && this.callbacks.before) this.callbacks.before();
			switch (code) {
				case 401:
					if (this.callbacks.on401Unauthorized)
						this.callbacks.on401Unauthorized();
					break;
				case 403:
					if (this.callbacks.on403Forbidden)
						this.callbacks.on403Forbidden();
					break;
				case 423:
					if (this.callbacks.on423Locked)
						this.callbacks.on423Locked();
					break;
				default:
					if (this.callbacks.onUnknownError)
						this.callbacks.onUnknownError();
			}
		}
	}
}
