import * as React from 'react'
import "./Index.view.less"
import AdminOnly from '../../containers/AdminOnly/AdminOnly';
import { inject } from 'mobx-react';
import { User } from '../../common/User';
import { nullable } from '../../utils/core';
import MainLayout from '../../containers/Main/Main.layout';
import TimeTable from '../../components/TimeTable/TimeTable';

let data: string[][] = [
	["高数", "", "大物", "Java程序设计", "", "C++语言设计", ""],
	["大物", "高数", "普通化学", "", "毛概", "数据库原理", ""],
	["", "理论力学", "", "编译原理", "", "", ""],
	["马克思原理", "", "", "普通化学", "数据库原理", "", ""],
	["", "", "毛概", "", "C#程序设计", "", ""],
]

@inject('user')
class IndexView extends React.PureComponent<{ user: User | nullable }> {
	render() {
		return <div className="index-view">
			<TimeTable data={data} className="timetable"/>
		</div>
	}
}

export default AdminOnly(MainLayout(IndexView))
