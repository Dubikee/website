import "./Login.view.less";
import * as React from 'react';
import { Row, Col, message, Form, Checkbox, Button, Input, Icon } from "antd";
import { request, setToken, nullable } from "../../utils/core";
import { inject, observer } from "mobx-react";
import { ServiceNames } from "src/services";
import { RouteComponentProps, withRouter } from "react-router";
import { runInAction } from "mobx";
import { LoginModel } from "../../common/LoginModel";
import { AuthStatus } from "../../common/models/AuthStatus";
import { Errors } from "../../common/config/Errors";
import { Infos } from "../../common/config/Infos";
import { User } from "../../common/stores/User";

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
	async login() {
		let user = this.props.user!;
		let { uid, pwd } = this.state;
		if (!(/^[0-9]{8,}$/).test(uid)) {
			message.error(Errors.UidIllegal)
			return
		}
		if (pwd.length < 8) {
			message.error(Errors.PwdTooShort)
			return
		}
		const hide = message.loading(Infos.Landing);
		try {
			let res = await request("/api/account/login")
				.forms(this.state)
				.post<LoginModel>();
			hide();
			if (res.status != 200) {
				message.error(Errors.ServerFailure);
				return;
			}
			let { status, uid, name, phone, email, role, jwt } = res.data
			switch (status) {
				case AuthStatus.Ok:
					setToken(jwt!)
					runInAction(() => {
						user.login = true;
						user.uid = uid!;
						user.name = name!;
						user.role = role!;
						user.phone = phone;
						user.email = email;
					})
					message.info(Infos.LoginSuccess, 1, () => {
						if (this.props.history.length > 0)
							this.props.history.goBack();
						else
							this.props.history.push('/home/index');
					})
					break;
				case AuthStatus.PasswordWrong:
					message.error(Errors.PwdWrong)
					break
				case AuthStatus.UIdNotFind:
					message.error(Errors.UidNotExist)
					break
				default:
					message.error(Errors.UnknownError)
					break
			}

		} catch (error) {
			hide()
			message.error(Errors.NetworkError)
		}
	}
	render() {
		return <div className="login-view">
			<Row className="form-row" type="flex" justify="center">
				<Col xs={18} sm={12} md={10} lg={8} xl={6} xxl={4} className='form-col'>
					<h1>LOGIN</h1>
					<Form className="login-form">
						<Form.Item>
							<Input onChange={e => { this.setState({ uid: e.target.value }) }} prefix={<Icon type="user" style={{ color: "rgba(0,0,0,.25)" }} />} placeholder="Account" />
						</Form.Item>
						<Form.Item>
							<Input onChange={e => { this.setState({ pwd: e.target.value }) }} prefix={<Icon type="lock" style={{ color: "rgba(0,0,0,.25)" }} />} type="password" placeholder="Password" />
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
