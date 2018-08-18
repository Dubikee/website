import * as React from 'react';
import './TimeTable.less';
import { Table, Tag, Popover, Tabs } from 'antd';
import { ColumnProps } from 'antd/lib/table';
import { Course } from '../../common/Course';
import { range } from '../../utils/core';
import { TimeTableItem } from '../../common/TimeTableItem';

interface ITimeTableProps {
	loading: boolean;
	showName: boolean,
	showTeacher: boolean,
	showLocation: boolean,
	className?: string;
	data: TimeTableItem[][];
}

const days = ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天"];


class TimeTable extends React.PureComponent<ITimeTableProps> {
	state = {
		week: 1
	}
	makeTable(week: number) {
		let table: {}[] = []
		for (let x = 0; x < 5; x++) {
			let row = {}
			for (let y = 0; y < 7; y++) {
				let day = `day${y}`;
				row['key'] = y;
				let { oddWeek, evenWeek } = this.props.data[x][y];
				if (oddWeek && week % 2 == 1)
					row[day] = oddWeek.start <= week && oddWeek.end >= week ? oddWeek : null
				else if (evenWeek && week % 2 == 0)
					row[day] = evenWeek.start <= week && evenWeek.end >= week ? evenWeek : null
			}
			table.push(row);
		}
		return table;
	}
	render() {
		let columns = days.map((day, i) => {
			return {
				title: day,
				dataIndex: `day${i}`,
				width: 60,
				align: 'center',
				render: course => {
					if (course) {
						let content = <div className='content'>
							<p style={{ margin: 0, lineHeight: '24px' }}>教室：{course['location']}</p>
							<p style={{ margin: 0, lineHeight: '24px' }}>老师：{course['teacher']}</p>
						</div>
						return <Popover content={content} title={course['name']}>
							<Tag color="lime" visible={this.props.showName}>{course['name']}</Tag>
							<Tag color="cyan" visible={this.props.showLocation}>{course['location']}</Tag>
							<Tag color="volcano" visible={this.props.showTeacher}>{course['teacher']}</Tag>
						</Popover>
					}
					else
						return <div>
							<br />
							<br />
						</div>
				}
			} as ColumnProps<Course>
		})
		return <Tabs defaultActiveKey={this.state.week.toString()}>
			{range(1, 19).map(x => {
				return <Tabs.TabPane key={x.toString()} tab={`第${x}周`}>
					<Table
						columns={columns}
						dataSource={this.makeTable(x)}
						pagination={false}
						className="timetable"
						loading={this.props.loading}
					/>
				</Tabs.TabPane>
			})}
		</Tabs>
	}
}

export default TimeTable;
