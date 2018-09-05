import { observable, action } from "mobx";
import { Course } from "../models/Course";

export class TableStore {
	loaded = true;
	@observable
	table: any[];
	constructor() {
		const txt = localStorage.getItem("table");
		if (txt) {
			const tb = <string[][]>JSON.parse(txt);
			if (tb) {
				this.table = makeTables(tb);
				this.loaded = true;
				return;
			}
		}
		this.table = makeTables(Array(5).fill(Array(7).fill("")));
	}
	@action.bound
	setTables(arr: string[][]) {
		this.table = makeTables(arr);
		this.loaded = true;
		localStorage.setItem("table", JSON.stringify(arr));
	}
}

function makeTables(arr: string[][]) {
	let source: (Course | null)[][] = [];
	for (let x = 0; x < 5; x++) {
		let row: (Course | null)[] = [];
		for (let y = 0; y < 7; y++) row.push(parseCourse(arr[x][y]));
		source.push(row);
	}
	let table = [];
	for (let x = 0; x < 5; x++) {
		let row = { key: x.toString() };
		for (let y = 0; y < 7; y++) {
			let day = `day${y}`;
			row[day] = source[x][y];
		}
		table.push(row);
	}
	return table;
}

function parseCourse(str: string) {
	const reg = /(.+)[@](.+)[◇](.+)/;

	if (reg.test(str)) {
		let group = reg.exec(str);
		if (group == null || group.length < 4) return null;
		return {
			name: group[1],
			location: group[2],
			time: group[3]
		} as Course;
	}
	return null;
}
