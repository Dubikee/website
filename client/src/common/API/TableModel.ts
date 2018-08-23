import { WhutStatus } from "../models/WhutStatus";
import { nullable } from "../../utils";
export class TableModel {
	status: WhutStatus;
	table: string[][] | nullable;
}