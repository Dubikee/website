import * as React from 'react';
import './TimeTable.less';
import { Table, Form, Switch } from 'antd';
import { ColumnProps } from 'antd/lib/table';

interface ITimeTableProps {
	className?: string
	data: string[][]
}

class TimeTableRow {
	key: number;
	"星期一": string;
	"星期二": string;
	"星期三": string;
	"星期四": string;
	"星期五": string;
	"星期六": string;
	"星期天": string
}

let keys = ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天"];

class TimeTable extends React.PureComponent<ITimeTableProps, any> {
	state = {
		displayName: true,
		displayTeacher: false,
		displayRoom: false,
	}
	render() {
		let data = this.props.data.map((row, x) => {
			return {
				key: x,
				"星期一": row[0],
				"星期二": row[1],
				"星期三": row[2],
				"星期四": row[3],
				"星期五": row[4],
				"星期六": row[5],
				"星期天": row[6]
			} as TimeTableRow;
		})
		let columns = keys.map(key => {
			return {
				title: key,
				dataIndex: key,
				width: 80,
				align: 'center',
				render: txt => {
					return <div>
						<p>{txt}</p>
					</div>
				}
			} as ColumnProps<TimeTableRow>
		})
		return <div className={this.props.className}>
			<Form layout='inline' className="timetable-form">
				<Form.Item label="显示名称">
					<Switch checked={this.state.displayName} onChange={e => this.setState({ a: e })} />
				</Form.Item>
				<Form.Item label="显示老师">
					<Switch checked={this.state.displayTeacher} onChange={e => this.setState({ a: e })} />
				</Form.Item>
				<Form.Item label="显示教室">
					<Switch checked={this.state.displayRoom} onChange={e => this.setState({ a: e })} />
				</Form.Item>
			</Form>
			<Table
				columns={columns}
				dataSource={data}
				pagination={false}
				className="timetable"
				bordered
			/>
		</div>
	}
}

export default TimeTable;
