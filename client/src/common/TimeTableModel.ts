import { WhutStatus } from "./WhutStatus";
import { nullable } from "../utils/core";

export class TimeTableModel {
	status: WhutStatus;
	timeTable: string[][] | nullable;
}
