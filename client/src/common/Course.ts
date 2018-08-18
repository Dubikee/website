import { nullable } from "../utils/core";

export class TimeTableItem {
	odd: Course | nullable;
	even: Course | nullable;
}

export class Course {
	name: string;
	start: number;
	end: number;
	teacher: string
	location: string;
}
