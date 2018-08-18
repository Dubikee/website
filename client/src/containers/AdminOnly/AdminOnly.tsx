import * as React from 'react'
import { inject } from 'mobx-react';
import { getToken, request, removeToken, nullable } from '../../utils/core';
import { runInAction } from 'mobx';
import { ValidateModel } from "../../common/ValidateModel";
import { AuthStatus } from "../../common/AuthStatus";
import { User } from '../../common/User';
import { message, Spin } from 'antd';
import { RouteComponentProps, withRouter } from 'react-router';
import { isMaster } from '../../utils/core';

interface IAdimOnlyPorps extends RouteComponentProps<any> {
	user: User | nullable
}

export default (View: any) => {
	@inject('user')
	class AdminOnly extends React.PureComponent<IAdimOnlyPorps> {
		state = {
			finished: false
		}
		componentWillMount() {
			let user = this.props.user!;
			//已经登陆
			if (user.login) {
				if (isMaster(user)) {
					this.setState({ finished: true })
				}
				else {
					message.error("权限不足！", 1, () => {
						if (this.props.history.length > 0)
							this.props.history.goBack()
						else
							this.props.history.push('/login')
					});
				}
				return;
			}
			//未登陆
			let token = getToken();
			if (!token) {
				this.props.history.push('/login')
				return;
			}
			request('/api/account/validate')
				.auth(token)
				.get<ValidateModel>()
				.then(res => {
					if (res.status != 200) {
						this.tologin("服务器故障");
						return;
					}
					let { status, uid, name, phone, email, role } = res.data
					switch (status) {
						case AuthStatus.Ok:
							let user = this.props.user!;
							runInAction(() => {
								user.login = true;
								user.uid = uid!;
								user.name = name!;
								user.role = role!;
								user.phone = phone;
								user.email = email;
							})
							setTimeout(() => {
								this.setState({ finished: true })
							}, 300);
							break;
						case AuthStatus.TokenExpired:
							removeToken();
							this.tologin("登陆过期,请重新登陆");
							break;
						default:
							removeToken();
							this.tologin("未知错误");
							break;
					}
				}).catch(() => {
					this.tologin("网络错误");
				})
		}
		tologin(msg: string) {
			message.error(msg, 1, () => {
				this.props.history.push('/login');
			})
		}
		render() {
			return this.state.finished ? <View /> : <div style={{
				textAlign: "center", paddingTop: "10em"
			}}>
				<Spin size="large" />
			</div>;
		}
	}
	return withRouter(AdminOnly);
}
