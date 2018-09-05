import { nullable } from "../../utils";
import { Rink } from "../../common/models/Rink";
import { BaseModel } from "./BaseModel";
export interface RinkModel extends BaseModel {
	rink: Rink | nullable;
}
