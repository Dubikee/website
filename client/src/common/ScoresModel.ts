import { WhutStatus } from "./WhutStatus";
import { ScoreInfo } from "./ScoreInfo";
import { GpaRinks } from "./GpaRinks";
import { nullable } from "../utils/core";

export interface ScoresModel {
	status: WhutStatus;
	scores: ScoreInfo[] | nullable;
	rinks: GpaRinks | nullable
}
