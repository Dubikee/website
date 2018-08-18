import * as React from 'react'
import "./Courses.view.less"
import { inject } from 'mobx-react';
import { User } from '../../../common/User';
import { nullable, request, getToken } from '../../../utils/core';
import TimeTable from '../../../components/TimeTable/TimeTable';
import { TimeTableModel } from '../../../common/TimeTableModel';
import { withRouter, RouteComponentProps } from 'react-router';
import { message, Form, Switch } from 'antd';
import { WhutStatus } from '../../../common/WhutStatus';
import { WhutStudent } from '../../../common/WhutStudent';


interface ICoursesViewProps extends RouteComponentProps<never> {
	user: User | nullable,
	student: WhutStudent | nullable
}


@inject('user', 'student')
class CoursesView extends React.PureComponent<ICoursesViewProps> {
	state = {
		loading: true,
		showName: true,
		showTeacher: false,
		showLocation: false
	}
	componentDidMount() {
		let { loadedTimeTable } = this.props.student!
		if (!loadedTimeTable)
			this.loadTimeTable();
	}
	loadTimeTable() {
		request('/api/whut/timetable')
			.auth(getToken()!)
			.get<TimeTableModel>()
			.then(res => {
				switch (res.status) {
					case 200:
						return res.data;
					case 401:
						message.error("登陆过期，请重新登陆", 2, () => {
							this.props.history.push('/login')
						})
						return null
					default:
						throw Error("请求失败")
				}
			})
			.then(model => {
				if (!model) return;
				switch (model.status) {
					case WhutStatus.Ok:
						this.props.student!.updateTimeTable(model.timeTable!)
						setInterval(() => {
							this.setState({ loading: false })
						}, 1000)
						break;
					case WhutStatus.StudentNotFind:
						break;
					case WhutStatus.PwdWrong:
						break;
					case WhutStatus.WhutServerCrashed:
						break;
					default:
						throw Error(`Status = ${model.status}`)
				}
			})
			.catch(err => {
				console.log(err)
				message.error("课表查询失败")
			})
	}

	render() {
		return <div className="courses-view">
			<Form layout='inline' className="timetable-form">
				<Form.Item label="显示名称">
					<Switch checked={this.state.showName} onChange={e => this.setState({ showName: e })} />
				</Form.Item>
				<Form.Item label="显示老师">
					<Switch checked={this.state.showTeacher} onChange={e => this.setState({ showTeacher: e })} />
				</Form.Item>
				<Form.Item label="显示教室">
					<Switch checked={this.state.showLocation} onChange={e => this.setState({ showLocation: e })} />
				</Form.Item>
			</Form>
			<TimeTable {...this.state} data={this.props.student!.timeTable} className="timetable" />
		</div >
	}
}

export default withRouter(CoursesView)
