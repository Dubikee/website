import * as React from 'react'
import { inject } from 'mobx-react';
import { getToken, removeToken, nullable, match, isRole } from '../../utils';
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

const auth = (requriedRole: 'admin' | 'vistor' | 'master') => (View: any) => {
	@inject('user')
	class AdminOnly extends React.PureComponent<IAdimOnlyPorps> {
		state = {
			finished: false
		}
		componentWillMount() {
			const { login, role } = this.props.user!;
			if (login)
				this.checkRole(role);
			else
				this.requestValidate();
		}
		checkRole(userRole: string) {
			isRole(userRole, requriedRole) ? this.setState({ finished: true }) :
				message.warn(Tips.PermissionDenied, () => {
					this.props.history.length > 0 ?
						this.props.history.goBack() :
						this.props.history.push('/login')
				});
		}
		async requestValidate() {
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
							const { status, ...info } = data
							runInAction(() => this.props.user!.updateUser({ login: true, ...info }))
							this.checkRole(info.role!);
						},
					[AuthStatus.TokenExpired]:
						() => {
							removeToken();
							message.warn(Tips.TokenExpires, () => {
								this.props.history.push('/login');
							})
						},
					['_']:
						() => message.error(Tips.UnknownError)
				})
				match({
					200: () => ok(data.status),
					401: () => this.props.history.push('/login'),
					'_': () => message.error(Tips.UnknownError)
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

const vistorRequired = auth('vistor');
const adminRequired = auth('admin');
const masterRequired = auth('master');

export { adminRequired, masterRequired, vistorRequired }
