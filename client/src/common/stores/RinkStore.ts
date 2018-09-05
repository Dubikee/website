import { observable, action } from "mobx";
import { nullable } from "../../utils";
import { Rink } from "../models/Rink";
export class RinkStore {
	@observable
	rink: Rink | nullable;
	constructor() {
		const txt = localStorage.getItem("rink");
		if (txt) {
			const rink = <Rink>JSON.parse(txt);
			if (rink) {
				this.rink = rink;
			}
		}
	}
	@action.bound
	setRink(rink: Rink) {
		this.rink = rink;
		localStorage.setItem("rink", JSON.stringify(rink));
	}
}
