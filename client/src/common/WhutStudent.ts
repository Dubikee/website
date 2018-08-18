import { observable, action } from "mobx";
import { nullable } from "../utils/core";
import { TimeTableItem, Course } from "./Course";

const reg = /(.+)[(]第([0-9]+)[-]([0-9]+)([单|双]?)周,(.+?),(.+?)[)]/;

export class WhutStudent {
	@observable
	studentId: string | nullable;
	@observable
	loadedTimeTable: boolean = false;
	@observable
	timeTable: TimeTableItem[][] = Array(5).fill(Array(7).fill({ odd: null, even: null } as TimeTableItem))

	@action.bound
	updateTimeTable(arr: string[][]) {
		for (let x = 0; x < arr.length; x++) {
			const row = arr[x];
			for (let y = 0; y < row.length; y++) {
				const col = row[y];
				let strs = col.split(" ");
				let item = this.timeTable[x][y];
				if (strs.length == 1 && reg.test(strs[0])) {
					let groups = reg.exec(strs[0])!;
					let info = {
						name: groups[1],
						start: parseInt(groups[2]),
						end: parseInt(groups[3]),
						teacher: groups[5],
						location: groups[6]
					} as Course
					item.odd = item.even = info;
				}
				else if (strs.length == 2) {
					for (const str of strs) {
						if (!reg.test(strs[0])) continue;
						let groups = reg.exec(str)!;
						let info = {
							name: groups[1],
							start: parseInt(groups[2]),
							end: parseInt(groups[3]),
							teacher: groups[5],
							location: groups[6]
						} as Course
						switch (groups[4]) {
							case '单':
								item.odd = info;
								break;
							case '双':
								item.even = info;
								break;
						}
					}
				}
			}
		}
		this.loadedTimeTable = true;
	}
}
