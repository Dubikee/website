import * as React from 'react'
import "./Courses.view.less"
import { inject, observer } from 'mobx-react';
import TimeTable from '../../../components/TimeTable/TimeTable';
import { withRouter, RouteComponentProps } from 'react-router';
import { message, Form, Switch, Icon, Button, Checkbox } from 'antd';
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
	async refresh() {
		this.setState({ loading: true })
		await this.loadTable();
	}
	render() {
		return <div className="courses-view">
			<TimeTable {...this.state} data={this.props.student!.tables} className="timetable" />
			<Form layout='inline' style={{
				paddingTop: 30,
				paddingBottom: 30,
				textAlign:'center'
			}}>
				<Form.Item label="显示名称">
					<Checkbox checked={this.state.showName} onChange={e => this.setState({ showName: e.target.checked })} />
				</Form.Item>
				<Form.Item label="显示老师">
					<Checkbox checked={this.state.showTeacher} onChange={e => this.setState({ showTeacher: e.target.checked })} />
				</Form.Item>
				<Form.Item label="显示教室">
					<Checkbox checked={this.state.showLocation} onChange={e => this.setState({ showLocation: e.target.checked })} />
				</Form.Item>
				<Form.Item>
					<Button size="small" type="dashed" onClick={() => this.refresh()} icon='reload'>刷新</Button>
				</Form.Item>
			</Form>
		</div >
	}
}

export default withRouter(CoursesView)
