import * as React from 'react';
import './TimeTable.view.less';

interface ITimeTableProps {
	data: string[][]
}

class TimeTable extends React.PureComponent<ITimeTableProps, any> {
	render() {
		return <div>
			TimeTable
        </div>
	}
}

export default TimeTable;
