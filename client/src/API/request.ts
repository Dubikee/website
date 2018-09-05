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
	Ok: (data: T) => any;
	before?: () => any;
	NotAllowed?: () => any;
	TokenExpired?: () => any;
	UidIllegal?: () => any;
	UidNotFind?: () => any;
	UidHasExist?: () => any;
	PwdWrong?: () => any;
	PwdIllegal?: () => any;
	WhutPwdWrong?: () => any;
	WhutIdNotFind?: () => any;
	InputIllegal?: () => any;
	WhutCrashed?: () => any;
	///请求错误
	Unauthorized401?: () => any;
	Forbidden403?: () => any;
	Locked423?: () => any;
	///未处理的错误
	UnknownError: () => any;
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
				`${this.url} response ok with data status ${data.status}`
			);
			if (this.callbacks.before) {
				this.callbacks.before();
				runBefore = true;
			}
			switch (data.status) {
				case Status.Ok:
					if (this.callbacks.Ok) this.callbacks.Ok(data);
					break;
				case Status.NotAllowed:
					if (this.callbacks.NotAllowed)
						this.callbacks.NotAllowed();
					break;
				case Status.TokenExpired:
					if (this.callbacks.TokenExpired)
						this.callbacks.TokenExpired();
					break;
				case Status.UidIllegal:
					if (this.callbacks.UidIllegal)
						this.callbacks.UidIllegal();
					break;
				case Status.UidNotFind:
					if (this.callbacks.UidNotFind)
						this.callbacks.UidNotFind();
					break;
				case Status.UidHasExist:
					if (this.callbacks.UidHasExist)
						this.callbacks.UidHasExist();
					break;
				case Status.PwdWrong:
					if (this.callbacks.PwdWrong) this.callbacks.PwdWrong();
					break;
				case Status.PwdIllegal:
					if (this.callbacks.PwdIllegal)
						this.callbacks.PwdIllegal();
					break;
				case Status.WhutPwdWrong:
					if (this.callbacks.WhutPwdWrong)
						this.callbacks.WhutPwdWrong();
					break;
				case Status.WhutCrashed:
					if (this.callbacks.WhutCrashed)
						this.callbacks.WhutCrashed();
					break;
				case Status.InputIllegal:
					if (this.callbacks.InputIllegal)
						this.callbacks.InputIllegal();
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
					if (this.callbacks.Unauthorized401)
						this.callbacks.Unauthorized401();
					break;
				case 403:
					if (this.callbacks.Forbidden403)
						this.callbacks.Forbidden403();
					break;
				case 423:
					if (this.callbacks.Locked423)
						this.callbacks.Locked423();
					break;
				default:
					if (this.callbacks.UnknownError)
						this.callbacks.UnknownError();
			}
		}
	}
}
