import * as React from 'react';
import './TimeTable.less';
import { Table } from 'antd';
import { ColumnProps } from 'antd/lib/table';

interface ITimeTableProps {
	className?: string
	data: string[][]
}

let arr = ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期天"];

class TimeTable extends React.PureComponent<ITimeTableProps, any> {
	render() {
		let data = this.props.data.map((row, x) => {
			let rowdata = {};
			arr.forEach((e, i) => {
				rowdata = { ...rowdata, [e]: row[i] }
			});
			return rowdata;
		})
		let columns = arr.map((v, i) => {
			return {
				title: v,
				dataIndex: v,
				className: i % 2 == 0 ? 'red' : 'blue'
			} as ColumnProps<never>
		})
		return <div className={this.props.className}>
			<Table
				columns={columns}
				dataSource={data}
				pagination={false}
				bordered
			/>
		</div>
	}
}

export default TimeTable;
