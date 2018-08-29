import * as React from 'react'
import { inject, observer } from 'mobx-react';
import TimeTable from '../../../components/TimeTable/TimeTable';
import { withRouter, RouteComponentProps } from 'react-router';
import { message, Form, Button, Checkbox, Modal } from 'antd';
import { nullable, getToken, removeToken, match, parseStatus } from '../../../utils';
import { WhutStudent } from '../../../common/stores/WhutStudent';
import { User } from '../../../common/stores/User';
import { request } from '../../../utils/request';
import { Tips } from '../../../common/config/Tips';
import { WhutStatus } from '../../../common/models/WhutStatus';
import "./Courses.view.less"

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
		const { tableLoaded } = this.props.student!
		if (tableLoaded)
			this.setState({ loading: false })
		else
			this.loadTable(true);
	}
	async loadTable(useServerCache: boolean) {
		try {
			const url = useServerCache ? '/api/whut/table' : '/api/whut/updatetable';
			const res = await request(url)
				.auth(getToken()!)
				.get();
			this.setState({ loading: false })
			if (res.status = 200) {
				const { status, table } = res.data;
				match(status)({
					[WhutStatus.Ok]:
						() => {
							const student = this.props.student!;
							if (table && table.length == 5 && table[0].length == 7) {
								student.setTables(table);
								message.info(Tips.Ok)
							}
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
						() => message.error(Tips.UnknownError)
				})
			}
			else {
				throw Error(res.statusText);
			}
		} catch (error) {
			this.setState({ loading: false })
			const status = parseStatus(error);
			if (status) {
				match(status)({
					401: () => message.error(Tips.TokenExpires, () => {
						removeToken();
						this.props.history.push('/login', { from: this.props.location.pathname });
					}),
					423: () => message.error(Tips.Locked),
					'_': () => message.error(Tips.UnknownError)
				})
			}
			else {
				console.log(error)
				message.error(Tips.ServerFailure)
			}
		}
	}
	refresh() {
		this.setState({ loading: true });
		Modal.confirm({
			title: '是否清空服务器缓存？',
			content: '清空缓存将对教务处重新发起请求',
			okText: '是',
			cancelText: '否',
			okType: 'danger',
			onOk: async () => await this.loadTable(false),
			onCancel: async () => await this.loadTable(true),
		});
	}
	render() {
		return <div>
			<TimeTable {...this.state} data={this.props.student!.tables} className="timetable" />
			<Form layout='inline' className='tb-settings'>
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
