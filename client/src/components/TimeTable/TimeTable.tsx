import * as React from "react";
import "./TimeTable.less";
import { Table, Tag, Popover, Tabs } from "antd";
import { ColumnProps } from "antd/lib/table";
import { Course } from "../../common/models/Course";

interface ITimeTableProps {
	loading: boolean;
	showName: boolean;
	showTeacher: boolean;
	showLocation: boolean;
	className?: string;
	data: {}[];
}

const days = [
	"星期一",
	"星期二",
	"星期三",
	"星期四",
	"星期五",
	"星期六",
	"星期天"
];

class TimeTable extends React.PureComponent<ITimeTableProps> {
	state = {
		week: 0
	};
	render() {
		let columns = days.map((day, i) => {
			return {
				title: day,
				dataIndex: `day${i}`,
				width: 60,
				align: "center",
				render: (course: Course) => {
					if (course) {
						const { location, name, time } = course;
						let content = (
							<div className="content">
								<p style={{ margin: 0, lineHeight: "30px" }}>
									教室： {location}
								</p>
								<p style={{ margin: 0, lineHeight: "30px" }}>
									时间： {time}
								</p>
							</div>
						);
						return (
							<Popover content={content} title={course["name"]}>
								<Tag
									color="geekblue"
									visible={this.props.showName}
								>
									{name}
								</Tag>
								<Tag
									color="cyan"
									visible={this.props.showLocation}
								>
									{location}
								</Tag>
							</Popover>
						);
					} else
						return (
							<div>
								<br />
								<br />
							</div>
						);
				}
			} as ColumnProps<Course>;
		});
		return (
			<Table
				columns={columns}
				dataSource={this.props.data}
				pagination={false}
				className="timetable"
				loading={this.props.loading}
			/>
		);
	}
}

export default TimeTable;
