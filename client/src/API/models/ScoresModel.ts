import { nullable } from "../../utils";
import { Score } from "../../common/models/Score";
import { BaseModel } from "./BaseModel";
export interface ScoresModel extends BaseModel {
	scores: Score[] | nullable;
}
