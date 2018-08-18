import * as React from 'react';
import './TimeTable.less';
import { Table, Tag, Select } from 'antd';
import { ColumnProps } from 'antd/lib/table';
import { TimeTableItem, Course } from '../../common/Course';
import { range } from '../../utils/core';

interface ITimeTableProps {
	loading: boolean;
	showName: boolean,
	showTeacher: boolean,
	showLocation: boolean,
	className?: string;
	data: TimeTableItem[][];
}

const days = ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天"];

class TimeTable extends React.PureComponent<ITimeTableProps, any> {
	state = {
		week: 1
	}
	makeTable(week: number) {
		return this.props.data.map((item, x) => {
			let tableRow = {}
			tableRow['key'] = x;
			for (let i = 0; i < item.length; i++) {
				let day = `day${i}`;
				let { odd, even } = item[i];
				if (week % 2 == 1 && odd)
					tableRow[day] = odd.start <= week && odd.end >= week ? odd : null
				else if (week % 2 == 0 && even)
					tableRow[day] = even.start <= week && even.end >= week ? even : null
			}
			return tableRow;
		})
	}
	render() {
		let columns = days.map((day, i) => {
			return {
				title: day,
				dataIndex: `day${i}`,
				width: 60,
				align: 'center',
				render: course => {
					return course ?
						<div>
							<Tag color="lime" visible={this.props.showName}>{course['name']}</Tag>
							<Tag color="cyan" visible={this.props.showLocation}>{course['location']}</Tag>
							<Tag color="volcano" visible={this.props.showTeacher}>{course['teacher']}</Tag>
						</div> 
						:
						<div>
							<br />
							<br />
						</div>
				}
			} as ColumnProps<Course>
		})
		let header = () => <div>
			<span style={{ lineHeight: '24px', fontSize: '16px' }}>课表</span>
			<Select size='small' onSelect={p => this.setState({ week: p })} defaultValue={this.state.week} style={{ float: 'right', width: 100 }}>
				{range(1, 19).map(x => <Select.Option key={x} value={x}>{`第${x}周`}</Select.Option>)}
			</Select>
		</div>
		return <Table
			columns={columns}
			dataSource={this.makeTable(this.state.week)}
			pagination={false}
			className="timetable"
			loading={this.props.loading}
			title={header}
			bordered
		/>

	}
}

export default TimeTable;
