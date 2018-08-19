import * as React from 'react'
import "./Courses.view.less"
import { inject, observer } from 'mobx-react';
import { User } from '../../../common/User';
import { nullable, request, getToken, removeToken } from '../../../utils/core';
import TimeTable from '../../../components/TimeTable/TimeTable';
import { TimeTableModel } from '../../../common/TimeTableModel';
import { withRouter, RouteComponentProps } from 'react-router';
import { message, Form, Switch, Icon } from 'antd';
import { WhutStatus } from '../../../common/WhutStatus';
import { WhutStudent } from '../../../common/WhutStudent';
import { Errors } from '../../../common/config/Errors';


interface ICoursesViewProps extends RouteComponentProps<never> {
	user: User | nullable,
	student: WhutStudent | nullable
}


@inject('user', 'student')
@observer
class CoursesView extends React.Component<ICoursesViewProps> {
	state = {
		loading: true,
		showName: true,
		showTeacher: true,
		showLocation: true
	}
	componentWillMount() {
		let { tableLoaded } = this.props.student!
		if (tableLoaded)
			this.setState({ loading: false })
		else
			this.loadTable();
	}
	async loadTable() {
		try {
			const res = await request('/api/whut/timetable')
				.auth(getToken()!)
				.get<TimeTableModel>();
			switch (res.status) {
				case 200:
					break;
				case 401:
					message.error(Errors.TokenExpires, () => {
						removeToken();
						this.props.history.push('/login')
					})
					return;
				default:
					throw Error(`HttpStatus=${res.status}`)
			}
			const { status, timeTable } = res.data;
			const student = this.props.student!;
			switch (status) {
				case WhutStatus.Ok:
					if (timeTable && timeTable.length == 5 && timeTable[0].length == 7)
						student.setTables(timeTable)
					else
						message.error(Errors.ServerFailure);
					break;
				case WhutStatus.StudentNotFind:
					message.error(Errors.NoStudent);
					break;
				case WhutStatus.PwdWrong:
					message.error(Errors.WhutPwdWrong);
					break;
				case WhutStatus.WhutServerCrashed:
					message.error(Errors.WhutServerCrashed);
					break;
				default:
					throw Error(`Status = ${status}`)
			}
			this.setState({ loading: false })

		} catch (error) {
			console.log(error)
			message.error(Errors.ServerFailure)
			this.setState({ loading: false })
		}
	}

	render() {
		return <div className="courses-view">
			<TimeTable {...this.state} data={this.props.student!.tables} className="timetable" />
			<Form layout='inline' className="timetable-form">
				<Form.Item label="显示名称">
					<Switch checkedChildren={<Icon type="check" />} checked={this.state.showName} onChange={e => this.setState({ showName: e })} />
				</Form.Item>
				<Form.Item label="显示老师">
					<Switch checkedChildren={<Icon type="check" />} checked={this.state.showTeacher} onChange={e => this.setState({ showTeacher: e })} />
				</Form.Item>
				<Form.Item label="显示教室">
					<Switch checkedChildren={<Icon type="check" />} checked={this.state.showLocation} onChange={e => this.setState({ showLocation: e })} />
				</Form.Item>
			</Form>
		</div >
	}
}

export default withRouter(CoursesView)
