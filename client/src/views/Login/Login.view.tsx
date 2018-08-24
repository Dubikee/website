import "./Login.view.less";
import * as React from 'react';
import { Row, Col, message, Form, Checkbox, Button, Input, Icon } from "antd";
import { setToken, nullable, match } from "../../utils";
import { inject, observer } from "mobx-react";
import { ServiceNames } from "src/services";
import { RouteComponentProps, withRouter } from "react-router";
import { runInAction } from "mobx";
import { User } from "../../common/stores/User";
import { Tips } from "../../common/config/Tips";
import { request } from "../../utils/request";
import { AuthStatus } from "../../common/models/AuthStatus";

interface IHomeViewProps extends RouteComponentProps<any> {
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
		if (!uid)
			message.warn(Tips.UidIsEmpty);
		if (!pwd)
			message.warn(Tips.PwdIsEmpty)
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
		let user = this.props.user!;
		const hide = message.loading(Tips.Landing);
		try {
			const { status, data } = await request("/api/account/login")
				.forms(this.state)
				.post();
			const ok = match({
				[AuthStatus.Ok]:
					() => {
						let { status, jwt, ...info } = data
						setToken(jwt!)
						runInAction(() => user.updateUser({ login: true, ...info }))
						message.info(Tips.LoginSuccess, 1, () => {
							if (this.props.history.length > 0)
								this.props.history.goBack();
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
			hide();
			match({
				200: () => ok(data.status),
				'_': () => message.error(Tips.ServerFailure)
			})(status)
		} catch (error) {
			hide()
			message.error(Tips.NetworkError)
		}
	}
	render() {
		return <div className="login-view">
			<Row className="form-row" type="flex" justify="center">
				<Col xs={18} sm={12} md={10} lg={8} xl={6} xxl={4} className='form-col'>
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
		</div>
	}
}

export default withRouter(LoginView);
