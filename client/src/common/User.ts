import { observable } from "mobx";

export class User {

	@observable
	public Uid: string

	@observable
	public Name: string

	@observable
	public Pwd: string

	@observable
	public Role: string

	@observable
	public Phone: string

	@observable
	public Email: string
}
