import { observable, action } from "mobx";
import { nullable } from "../../utils";

export class User {
	@observable
	public login: boolean = false;

	@observable
	public uid: string

	@observable
	public name: string

	@observable
	public role: string

	@observable
	public phone: string | nullable

	@observable
	public email: string | nullable

	@action.bound
	public updateUser(u: any) {
		for (const k in u)
			if (u[k]) this[k] = u[k]
	}
}
