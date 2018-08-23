import * as React from 'react'
import "./Courses.view.less"
import { inject, observer } from 'mobx-react';
import TimeTable from '../../../components/TimeTable/TimeTable';
import { withRouter, RouteComponentProps } from 'react-router';
import { message, Form, Switch, Icon } from 'antd';
import { nullable, getToken, removeToken, match } from '../../../utils';
import { WhutStudent } from '../../../common/stores/WhutStudent';
import { User } from '../../../common/stores/User';
import { request } from '../../../utils/request';
import { Tips } from '../../../common/config/Tips';
import { WhutStatus } from '../../../common/models/WhutStatus';

interface ICoursesViewProps extends RouteComponentProps<any> {
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
			const { status, data } = await request('/api/whut/table')
				.auth(getToken()!)
				.get();
			let ok = match({
				[WhutStatus.Ok]:
					() => {
						const student = this.props.student!;
						const { table } = data;
						if (table && table.length == 5 && table[0].length == 7)
							student.setTables(table)
						else
							message.error(Tips.ServerFailure);
					},
				[WhutStatus.StudentNotFind]:
					() => message.warn(Tips.NoStudent),
				[WhutStatus.PwdWrong]:
					() => message.warn(Tips.WhutPwdWrong),
				[WhutStatus.WhutServerCrashed]:
					() => message.error(Tips.WhutServerCrashed),
				['_']:
					() => { throw Error(`Status = ${status}`) },
				['*']:
					() => this.setState({ loading: false })
			})
			match({
				200: () => ok(data.status),
				401: () => message.error(Tips.TokenExpires, () => {
					removeToken();
					this.props.history.push('/login')
				}),
				'_': () => { throw Error(`HttpStatus=${status}`) }
			})(status)
		} catch (error) {
			console.log(error)
			message.error(Tips.ServerFailure)
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
