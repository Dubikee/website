import { observable, action } from "mobx";
import { Score } from "../models/Score";
export class ScoresStore {
	@observable
	scores: Score[] = [];
	constructor() {
		const txt = localStorage.getItem("scores");
		if (txt) {
			const scores = <Score[]>JSON.parse(txt);
			if (scores) {
				this.scores = scores;
			}
		}
	}
	@action.bound
	setScores(scores: Score[]) {
		this.scores = scores;
		localStorage.setItem("scores", JSON.stringify(scores));
	}
}
