import { WhutStatus } from "./models/WhutStatus";
import { ScoreInfo } from "./models/ScoreInfo";
import { GpaRinks } from "./models/GpaRinks";
import { nullable } from "../utils/core";

export interface ScoresModel {
	status: WhutStatus;
	scores: ScoreInfo[] | nullable;
	rinks: GpaRinks | nullable
}
