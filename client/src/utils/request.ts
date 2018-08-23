import Axios, { AxiosRequestConfig } from 'axios';
import * as qs from 'querystring';
import { API } from '../common/API';

export let request = <URL extends keyof API>(url: URL) => new HttpClient<API[URL]>(url)

class HttpClient<T> {
	private headers: any;
	private reqData: any;
	constructor(private url: string) { }
	public header(key: string, value: string) {
		if (this.headers)
			this.headers[key] = value;
		else
			this.headers = { [key]: value }
		return this;
	}
	public form(key: string, value: string) {
		if (this.reqData)
			this.reqData[key] = value;
		else
			this.reqData = { [key]: value }
		return this;
	}
	public forms(data: any) {
		this.reqData = data
		return this;
	}
	public auth(jwt: string) {
		return this.header('Authorization', 'Bearer ' + jwt)
	}
	public async get() {
		let config: AxiosRequestConfig = {}
		if (this.headers)
			config.headers = this.headers;
		if (this.reqData)
			config.params = this.reqData;
		return await Axios.get<T>(this.url, config)
	}
	public async post() {
		let config: AxiosRequestConfig = {}
		if (this.headers)
			config.headers = this.headers;
		return await Axios.post<T>(this.url, qs.stringify(this.reqData), config)
	}
}
