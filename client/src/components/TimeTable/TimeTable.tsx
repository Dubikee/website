import * as React from 'react';
import './TimeTable.less';
import { Table, Tag, Popover, Tabs } from 'antd';
import { ColumnProps } from 'antd/lib/table';
import { Course } from '../../common/Course';

interface ITimeTableProps {
	loading: boolean;
	showName: boolean,
	showTeacher: boolean,
	showLocation: boolean,
	className?: string;
	data: {}[][];
}

const days = ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天"];


class TimeTable extends React.PureComponent<ITimeTableProps> {
	state = {
		week: 0
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
							<p style={{ margin: 0, lineHeight: '30px' }}>教室：{course['location']}</p>
							<p style={{ margin: 0, lineHeight: '30px' }}>老师：{course['teacher']}</p>
						</div>
						return <Popover content={content} title={course['name']}>
							<Tag color="geekblue" visible={this.props.showName}>{course['name']}</Tag>
							<Tag color="cyan" visible={this.props.showLocation}>{course['location']}</Tag>
							<Tag color="lime" visible={this.props.showTeacher}>{course['teacher']}</Tag>
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
			{Array(18).fill(0).map((_, i) => {
				return <Tabs.TabPane key={i.toString()} tab={`第${i + 1}周`}>
					<Table
						columns={columns}
						dataSource={this.props.data[i]}
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
