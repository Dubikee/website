import { observable } from "mobx";
import { nullable } from "../utils/core";

export class User {
	@observable
	public login = false;

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
}
