import { nullable } from "../../utils";
import { BaseModel } from "./BaseModel";
export class TableModel extends BaseModel {
	table: string[][] | nullable;
}
