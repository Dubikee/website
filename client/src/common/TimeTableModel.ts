import { WhutStatus } from "./models/WhutStatus";
import { nullable } from "../utils/core";

export class TimeTableModel {
	status: WhutStatus;
	timeTable: string[][] | nullable;
}
