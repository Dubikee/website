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
import { Errors } from '../../common/config/Errors';

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
			if (user.login) {
				if (isMaster(user)) {
					this.setState({ finished: true })
				}
				else {
					message.error(Errors.PermissionDenied, () => {
						this.props.history.length > 0 ?
							this.props.history.goBack() :
							this.props.history.push('/login')
					});
				}
			}
			else {
				this.validate();
			}
		}
		async validate() {
			let token = getToken();
			if (!token) {
				this.props.history.push('/login')
				return;
			}
			try {
				const res = await request('/api/account/validate')
					.auth(token)
					.get<ValidateModel>()
				switch (res.status) {
					case 200:
						break;
					case 401:
						this.props.history.push('/login');
						return;
					default:
						throw Error();
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
						this.tologin(Errors.TokenExpires);
						break;
				}
			} catch (error) {
				console.log(error);
				message.error(Errors.NetworkError);
			}
		}
		tologin(msg: string) {
			message.error(msg, 1, () => {
				this.props.history.push('/login');
			})
		}
		render() {
			return this.state.finished ? <View /> : <div style={{
				textAlign: "center", paddingTop: "100px"
			}}>
				<Spin size="large" />
			</div>;
		}
	}
	return withRouter(AdminOnly);
}
