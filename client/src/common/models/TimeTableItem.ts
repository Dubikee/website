import { nullable } from "../../utils";
import { Course } from "./Course";
export class TimeTableItem {
	oddWeek: Course | nullable;
	evenWeek: Course | nullable;
}