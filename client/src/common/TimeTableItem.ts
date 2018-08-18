import { nullable } from "../utils/core";
import { Course } from "./Course";
export class TimeTableItem {
	oddWeek: Course | nullable;
	evenWeek: Course | nullable;
}