import * as React from "react";
import { Row, Col, message, Form, Checkbox, Button, Input, Icon } from "antd";
import { setToken, nullable } from "../../utils";
import { inject, observer } from "mobx-react";
import { RouteComponentProps, withRouter } from "react-router";
import { runInAction } from "mobx";
import { User } from "../../common/stores/User";
import { Tips } from "../../common/config/Tips";
import { request } from "../../API/request";
import "./Login.view.less";

interface IHomeViewProps extends RouteComponentProps<{ from: string | nullable }> {
	user: User | nullable;
}

@inject("user")
@observer
class LoginView extends React.Component<IHomeViewProps> {
	state = {
		uid: "",
		pwd: ""
	};
	check() {
		const { uid, pwd } = this.state;
		if (!uid) {
			message.warn(Tips.UidIsEmpty);
			return false;
		}
		if (!pwd) {
			message.warn(Tips.PwdIsEmpty);
			return false;
		}
		if (!/^[0-9]{8,}$/.test(uid)) {
			message.warn(Tips.UidIllegal);
			return false;
		}
		if (pwd.length < 8) {
			message.warn(Tips.PwdTooShort);
			return false;
		}
		return true;
	}
	async login() {
		if (!this.check()) return;
		const user = this.props.user!;
		const hide = message.loading(Tips.Landing);
		await request("/api/account/login")
			.forms(this.state)
			.on({
				before: () => {
					hide();
				},
				Ok: ({ status, jwt, ...info }) => {
					setToken(jwt!);
					runInAction(() =>
						user.updateUser({ login: true, ...info })
					);
					message.info(Tips.LoginSuccess, 1, () => {
						const { state } = this.props.location;
						if (state && state["from"])
							this.props.history.push(state["from"]);
						else this.props.history.push("/home/index");
					});
				},
				UnknownError: () => {
					message.error(Tips.NetworkError);
				},
				PwdWrong: () => {
					message.error(Tips.PwdWrong);
				},
				UidNotFind: () => {
					message.error(Tips.UidNotExist);
				},
				Locked423: () => {
					message.error(Tips.Locked);
				}
			})
			.post();
	}
	render() {
		return (
			<div className="login-view">
				<Row className="form-wrapper" type="flex" justify="center">
					<Col
						xs={18}
						sm={12}
						md={10}
						lg={8}
						xl={6}
						xxl={4}
						className="form-content"
					>
						<h1>LOGIN</h1>
						<Form className="login-form">
							<Form.Item>
								<Input
									size="large"
									onChange={e => {
										this.setState({ uid: e.target.value });
									}}
									prefix={
										<Icon
											type="user"
											style={{ color: "rgba(0,0,0,.25)" }}
										/>
									}
									placeholder="Account"
								/>
							</Form.Item>
							<Form.Item>
								<Input
									size="large"
									onChange={e => {
										this.setState({ pwd: e.target.value });
									}}
									prefix={
										<Icon
											type="lock"
											style={{ color: "rgba(0,0,0,.25)" }}
										/>
									}
									type="password"
									placeholder="Password"
								/>
							</Form.Item>
							<Form.Item className="form-item-last">
								<Checkbox>Remember me</Checkbox>
								<br />
								<Button
									onClick={() => this.login()}
									type="primary"
									htmlType="submit"
									className="login-form-button"
								>
									{" "}
									Log in{" "}
								</Button>
							</Form.Item>
						</Form>
					</Col>
				</Row>
				<div className="login-view-footer">
					<p>
						2017-2018 Made with{" "}
						<span style={{ color: "red" }}>❤</span> by Dubikee
					</p>
				</div>
			</div>
		);
	}
}

export default withRouter(LoginView);
