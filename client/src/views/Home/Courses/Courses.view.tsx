import * as React from "react";
import { inject, observer } from "mobx-react";
import TimeTable from "../../../components/TimeTable/TimeTable";
import { withRouter, RouteComponentProps } from "react-router";
import { message, Form, Button, Checkbox, Modal } from "antd";
import { nullable, getToken, removeToken } from "../../../utils";
import { User } from "../../../common/stores/User";
import { request } from "../../../API/request";
import { Tips } from "../../../common/config/Tips";
import "./Courses.view.less";
import { TableStore } from "../../../common/stores/TableStore";

interface ICoursesViewProps extends RouteComponentProps<any> {
	user: User | nullable;
	tableStore: TableStore | nullable;
}

@inject("user", "tableStore")
@observer
class CoursesView extends React.Component<ICoursesViewProps> {
	state = {
		loading: true,
		showName: true,
		showLocation: true
	};
	componentWillMount() {
		const { loaded } = this.props.tableStore!;
		if (loaded) this.setState({ loading: false });
		else this.loadTable(true);
	}
	async loadTable(reload: boolean = false) {
		await request("/api/whut/table")
			.form("reload", reload)
			.auth(getToken()!)
			.on({
				before: () => {
					this.setState({ loading: false });
				},
				Ok: dat => {
					const student = this.props.tableStore!!;
					const { table } = dat;
					if (table && table.length == 5 && table[0].length == 7) {
						student.setTables(table);
						message.info(Tips.Ok);
					} else message.error(Tips.ServerFailure);
				},
				UnknownError: () => {
					message.error(Tips.ServerFailure);
				},
				WhutIdNotFind: () => {
					message.warn(Tips.NoStudent);
				},
				WhutPwdWrong: () => {
					message.warn(Tips.WhutPwdWrong);
				},
				WhutCrashed: () => {
					message.error(Tips.WhutServerCrashed);
				},
				Unauthorized401: () => {
					message.error(Tips.TokenExpires, () => {
						removeToken();
						this.props.history.push("/login", {
							from: this.props.location.pathname
						});
					});
				},
				Locked423: () => {
					message.error(Tips.Locked);
				}
			})
			.get();
	}
	refresh() {
		this.setState({ loading: true });
		Modal.confirm({
			title: "是否清空服务器缓存？",
			content: "清空缓存将对教务处重新发起请求",
			okText: "是",
			cancelText: "否",
			okType: "danger",
			onOk: async () => await this.loadTable(true),
			onCancel: async () => await this.loadTable(false)
		});
	}
	render() {
		return (
			<div style={{ margin: "0px 15px 5px 15px" }}>
				<Form layout="inline" className="tb-settings">
					<Form.Item label="显示名称">
						<Checkbox
							checked={this.state.showName}
							onChange={e =>
								this.setState({ showName: e.target.checked })
							}
						/>
					</Form.Item>
					<Form.Item label="显示教室">
						<Checkbox
							checked={this.state.showLocation}
							onChange={e =>
								this.setState({
									showLocation: e.target.checked
								})
							}
						/>
					</Form.Item>
					<Form.Item style={{ float: 'right' }}>
						<Button
							type="dashed"
							onClick={() => this.refresh()}
							icon="reload"
						>
							刷新
						</Button>
					</Form.Item>
				</Form>

				<TimeTable
					{...this.state}
					data={this.props.tableStore!.table}
					className="timetable"
				/>
			</div>
		);
	}
}

export default withRouter(CoursesView);
