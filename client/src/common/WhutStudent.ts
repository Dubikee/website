import { observable } from "mobx";
import { nullable } from "../utils/core";
import { Course } from "./Course";
import { ScoreInfo } from "./ScoreInfo";
import { GpaRinks } from "./GpaRinks";
import { TimeTableItem } from "./TimeTableItem";

const reg = /(.+)[(]第([0-9]+)[-]([0-9]+)([单|双]?)周,(.+?),(.+?)[)]/;

function parseCourse(str: string) {
	let course: TimeTableItem = { oddWeek: null, evenWeek: null }
	if (!str || str.trim() === '') return course;
	let strs = str.split(' ');
	if (strs.length == 1) {
		if (reg.test(strs[0])) {
			let groups = reg.exec(strs[0])!;
			let c = {
				name: groups[1],
				start: parseInt(groups[2]),
				end: parseInt(groups[3]),
				teacher: groups[5],
				location: groups[6]
			} as Course;
			course.evenWeek = course.oddWeek = c;
		}
	}
	else if (strs.length == 2) {
		if (reg.test(strs[0]) && reg.test(strs[1])) {
			let first = reg.exec(strs[0])!;
			let last = reg.exec(strs[1])!;
			let c1 = {
				name: first[1],
				start: parseInt(first[2]),
				end: parseInt(first[3]),
				teacher: first[5],
				location: first[6]
			} as Course;
			let c2 = {
				name: last[1],
				start: parseInt(last[2]),
				end: parseInt(last[3]),
				teacher: last[5],
				location: last[6]
			} as Course;
			if (first[4] == '单' && last[4] == '双') {
				course.oddWeek = c1;
				course.evenWeek = c2;
			}
			else if (first[4] == '双' && last[4] == '单') {
				course.oddWeek = c2;
				course.evenWeek = c1;
			}
		}
	}
	return course;
}

export function makeTables(arr: string[][]) {
	let source: TimeTableItem[][] = []
	for (let x = 0; x < 5; x++) {
		let row: TimeTableItem[] = []
		for (let y = 0; y < 7; y++) {
			row.push(parseCourse(arr[x][y]))
		}
		source.push(row);
	}
	return source;
}

export class WhutStudent {
	@observable
	studentId: string | nullable;
	@observable
	tableLoaded: boolean = false;
	@observable
	scores: ScoreInfo[] | nullable;
	@observable
	rinks: GpaRinks | nullable
	@observable
	tables: TimeTableItem[][] = Array(5).fill(Array(7).fill({ oddWeek: null, evenWeek: null }));
}
