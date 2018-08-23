import { WhutStatus } from "../models/WhutStatus";
import { Score } from "../models/Score";
import { nullable } from "../../utils";
import { Rink } from "../models/Rink";
export interface ScoresRinkModel {
	status: WhutStatus;
	scores: Score[] | nullable;
	rink: Rink | nullable;
}