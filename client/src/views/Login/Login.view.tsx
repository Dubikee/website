import "./Login.view.less";
import * as React from 'react';
import MainLayout from "../../containers/Main/Main.layout";
import { Row, Col, message, Form, Checkbox, Button, Input, Icon } from "antd";
import { request, setToken, nullable } from "../../utils/core";
import { inject } from "mobx-react";
import { ServiceTypes } from "src/services";
import { RouteComponentProps, withRouter } from "react-router";
import { runInAction } from "mobx";
import { ILoginModel } from "../../common/ILoginModel";
import { User } from "../../common/User";
import { AuthStatus } from "../../common/Status";

interface IHomeViewProps extends RouteComponentProps<any> {
	user: User | nullable
}

@inject(ServiceTypes.user)
class LoginView extends React.PureComponent<IHomeViewProps> {
	state = {
		uid: "",
		pwd: "",
	};
	async login() {
		let user = this.props.user!;
		let { uid, pwd } = this.state;
		let reg = /^[0-9]{8,}$/
		if (!reg.test(uid)) {
			message.error("账号是8位以上的数字！")
			return
		}
		if (pwd.length < 6) {
			message.error("密码不合法！")
			return
		}
		const hide = message.loading('正在登陆...', 0);
		try {
			let res = await request("/api/account/login")
				.forms(this.state)
				.post<ILoginModel>();
			hide();
			if (res.status != 200) {
				message.error("服务器故障");
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
					message.info("登陆成功", 1, () => {
						this.props.history.push("/index")
					})
					break;
				case AuthStatus.PasswordWrong:
					message.error("账号或密码错误")
					break
				case AuthStatus.UIdNotFind:
					message.error("账号不存在,请重新输入")
					break
				default:
					message.error("未知错误，登陆失败")
					break
			}

		} catch (error) {
			message.error("网络错误")
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

export default MainLayout(withRouter(LoginView));
