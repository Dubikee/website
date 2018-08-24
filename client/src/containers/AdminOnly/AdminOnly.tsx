import * as React from 'react'
import { inject } from 'mobx-react';
import { getToken, removeToken, nullable, match, isAdmin } from '../../utils';
import { runInAction } from 'mobx';
import { message, Spin } from 'antd';
import { RouteComponentProps, withRouter } from 'react-router';
import { Tips } from '../../common/config/Tips';
import { request } from '../../utils/request';
import { User } from '../../common/stores/User';
import { AuthStatus } from '../../common/models/AuthStatus';

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
			const user = this.props.user!;
			if (user.login)
				isAdmin(user) ? this.setState({ finished: true }) :
					message.warn(Tips.PermissionDenied, () => {
						this.props.history.length > 0 ?
							this.props.history.goBack() :
							this.props.history.push('/login')
					});
			else
				this.validate();
		}
		async validate() {
			const token = getToken();
			if (!token) {
				this.props.history.push('/login')
				return;
			}
			try {
				const { status, data } = await request('/api/account/validate')
					.auth(token)
					.get()
				const ok = match({
					[AuthStatus.Ok]:
						() => {
							let user = this.props.user!;
							let { status, ...info } = data
							runInAction(() => user.updateUser({ login: true, ...info }))
							setTimeout(() => this.setState({ finished: true }), 300);
						},
					[AuthStatus.TokenExpired]:
						() => {
							removeToken();
							message.warn(Tips.TokenExpires, () => {
								this.props.history.push('/login');
							})
						}
				})
				match({
					200: () => ok(data.status),
					401: () => this.props.history.push('/login'),
					'_': () => { throw Error(status.toString()) }
				})(status)
			} catch (error) {
				console.log(error);
				message.error(Tips.NetworkError);
			}
		}
		render() {
			return this.state.finished ? <View /> : <div style={{
				textAlign: "center",
				paddingTop: "100px",
				backgroundColor: '#fff',
			}}>
				<Spin size="large" />
			</div>;
		}
	}
	return withRouter(AdminOnly);
}
