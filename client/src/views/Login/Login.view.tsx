import * as React from 'react';
import { Row, Col, message, Form, Checkbox, Button, Input, Icon } from "antd";
import { setToken, nullable, match, parseStatus } from "../../utils";
import { inject, observer } from "mobx-react";
import { ServiceNames } from "src/services";
import { RouteComponentProps, withRouter } from "react-router";
import { runInAction } from "mobx";
import { User } from "../../common/stores/User";
import { Tips } from "../../common/config/Tips";
import { request } from "../../utils/request";
import { AuthStatus } from "../../common/models/AuthStatus";
import "./Login.view.less";

interface IHomeViewProps extends RouteComponentProps<{ from: string | nullable }> {
	user: User | nullable
}

@inject(ServiceNames.user)
@observer
class LoginView extends React.Component<IHomeViewProps> {
	state = {
		uid: "",
		pwd: "",
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
		if (!(/^[0-9]{8,}$/).test(uid)) {
			message.warn(Tips.UidIllegal)
			return false;
		}
		if (pwd.length < 8) {
			message.warn(Tips.PwdTooShort)
			return false;
		}
		return true;
	}
	async login() {
		if (!this.check()) return;
		const user = this.props.user!;
		const hide = message.loading(Tips.Landing);
		try {
			const res = await request("/api/account/login")
				.forms(this.state)
				.post();
			hide();
			if (res.status == 200) {
				const { status, jwt, ...info } = res.data;
				match(status)({
					[AuthStatus.Ok]:
						() => {
							setToken(jwt!)
							runInAction(() => user.updateUser({ login: true, ...info }))
							message.info(Tips.LoginSuccess, 1, () => {
								const { state } = this.props.location;
								if (state && state['from'])
									this.props.history.push(state['from']);
								else
									this.props.history.push('/home/index');
							})
						},
					[AuthStatus.PasswordWrong]:
						() => message.error(Tips.PwdWrong),
					[AuthStatus.UIdNotFind]:
						() => message.error(Tips.UidNotExist),
					['_']:
						() => message.error(Tips.UnknownError),
				})
			}
			else {
				throw Error(res.statusText);
			}
		} catch (error) {
			hide();
			const status = parseStatus(error);
			if (status) {
				match(status)({
					423: () => message.error(Tips.Locked),
					'_': () => message.error(Tips.UnknownError)
				})
			}
			else {
				console.log(error);
				message.error(Tips.NetworkError);
			}
		}
	}
	render() {
		return <div className="login-view">
			<Row className="form-wrapper" type="flex" justify="center">
				<Col xs={18} sm={12} md={10} lg={8} xl={6} xxl={4} className='form-content'>
					<h1>LOGIN</h1>
					<Form className="login-form">
						<Form.Item>
							<Input size='large' onChange={e => { this.setState({ uid: e.target.value }) }} prefix={<Icon type="user" style={{ color: "rgba(0,0,0,.25)" }} />} placeholder="Account" />
						</Form.Item>
						<Form.Item>
							<Input size='large' onChange={e => { this.setState({ pwd: e.target.value }) }} prefix={<Icon type="lock" style={{ color: "rgba(0,0,0,.25)" }} />} type="password" placeholder="Password" />
						</Form.Item>
						<Form.Item className="form-item-last">
							<Checkbox>Remember me</Checkbox>
							<br />
							<Button onClick={() => this.login()} type="primary" htmlType="submit" className="login-form-button" > Log in </Button>
						</Form.Item>
					</Form>
				</Col>
			</Row>
			<div className='login-view-footer'>
				<p>2017-2018 Made with <span style={{ color: 'red' }}>❤</span> by Dubikee</p>
			</div>
		</div>
	}
}

export default withRouter(LoginView);
