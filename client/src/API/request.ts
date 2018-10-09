import Axios, { AxiosRequestConfig, AxiosResponse } from "axios";
import * as qs from "querystring";
import { API } from ".";
import { BaseModel } from "./models/BaseModel";
import { parseStatus } from "../utils";

export let request = <URL extends keyof API>(url: URL) =>
	new HttpClient<API[URL]>(url);

export enum Status {
	UnknownError,
	Ok,
	NotAllowed,
	TokenExpired,
	UidIllegal,
	UidNotFind,
	UidHasExist,
	PwdWrong,
	PwdIllegal,
	WhutPwdWrong,
	WhutIdNotFind,
	InputIllegal,
	WhutCrashed
}

interface IResponseCallBack<T> {
	ok: (data: T) => any;
	before?: () => any;
	notAllowed?: () => any;
	tokenExpired?: () => any;
	uidIllegal?: () => any;
	uidNotFind?: () => any;
	uidHasExist?: () => any;
	pwdWrong?: () => any;
	pwdIllegal?: () => any;
	whutPwdWrong?: () => any;
	whutIdNotFind?: () => any;
	inputIllegal?: () => any;
	whutCrashed?: () => any;
	///请求错误
	unauthorized401?: () => any;
	forbidden403?: () => any;
	locked423?: () => any;
	///未处理的错误
	unknownError: () => any;
}

class HttpClient<T extends BaseModel> {
	private headers: { [key: string]: any };
	private data: { [key: string]: any };
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

	public on(callbacks: IResponseCallBack<T>) {
		this.callbacks = callbacks;
		return this;
	}

	public async get() {
		await this.request();
	}
	public async post() {
		await this.request("post");
	}

	private async request(method: "get" | "post" = "get") {
		let runBefore = false;
		try {
			let res: AxiosResponse<T>;
			let config: AxiosRequestConfig = {};
			if (this.headers) config.headers = this.headers;
			switch (method) {
				case "get":
					if (this.data) config.params = this.data;
					res = await Axios.get<T>(this.url, config);
					break;
				case "post":
					res = await Axios.post<T>(
						this.url,
						qs.stringify(this.data),
						config
					);
					break;
				default:
					return;
			}
			const { data } = res;
			console.log(
				`${this.url} response ok with status ${data.status}`
			);
			if (this.callbacks.before) {
				this.callbacks.before();
				runBefore = true;
			}
			switch (data.status) {
				case Status.Ok:
					if (this.callbacks.ok)
						this.callbacks.ok(data);
					break;
				case Status.NotAllowed:
					if (this.callbacks.notAllowed)
						this.callbacks.notAllowed();
					break;
				case Status.TokenExpired:
					if (this.callbacks.tokenExpired)
						this.callbacks.tokenExpired();
					break;
				case Status.UidIllegal:
					if (this.callbacks.uidIllegal)
						this.callbacks.uidIllegal();
					break;
				case Status.UidNotFind:
					if (this.callbacks.uidNotFind)
						this.callbacks.uidNotFind();
					break;
				case Status.UidHasExist:
					if (this.callbacks.uidHasExist)
						this.callbacks.uidHasExist();
					break;
				case Status.PwdWrong:
					if (this.callbacks.pwdWrong)
						this.callbacks.pwdWrong();
					break;
				case Status.PwdIllegal:
					if (this.callbacks.pwdIllegal)
						this.callbacks.pwdIllegal();
					break;
				case Status.WhutPwdWrong:
					if (this.callbacks.whutPwdWrong)
						this.callbacks.whutPwdWrong();
					break;
				case Status.WhutCrashed:
					if (this.callbacks.whutCrashed)
						this.callbacks.whutCrashed();
					break;
				case Status.InputIllegal:
					if (this.callbacks.inputIllegal)
						this.callbacks.inputIllegal();
					break;
			}
		} catch (error) {
			const code = parseStatus(error);
			console.log(
				`${this.url} response error with HttpStatusCode ${code}`
			);
			if (!runBefore && this.callbacks.before) this.callbacks.before();
			switch (code) {
				case 401:
					if (this.callbacks.unauthorized401)
						this.callbacks.unauthorized401();
					break;
				case 403:
					if (this.callbacks.forbidden403)
						this.callbacks.forbidden403();
					break;
				case 423:
					if (this.callbacks.locked423)
						this.callbacks.locked423();
					break;
				default:
					if (this.callbacks.unknownError)
						this.callbacks.unknownError();
			}
		}
	}
}

