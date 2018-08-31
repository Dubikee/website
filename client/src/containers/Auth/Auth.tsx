import * as React from 'react'
import { inject, observer } from 'mobx-react';
import { getToken, removeToken, nullable, match, isRole, parseStatus } from '../../utils';
import { runInAction } from 'mobx';
import { message, Spin } from 'antd';
import { RouteComponentProps, withRouter } from 'react-router';
import { Tips } from '../../common/config/Tips';
import { request } from '../../utils/request';
import { User } from '../../common/stores/User';
import { AuthStatus } from '../../common/models/AuthStatus';
import Axios from 'axios';

interface IAdimOnlyPorps extends RouteComponentProps<any> {
	user: User | nullable
}

const auth = (requriedRole: 'admin' | 'vistor' | 'master') => (View: any) => {
	@inject('user')
	@observer
	class AdminOnly extends React.Component<IAdimOnlyPorps> {
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
				message.warn(Tips.PermissionDenied, () => this.gotologin());
		}
		gotologin() {
			this.props.history.push('/login', { from: this.props.location.pathname });
		}
		async requestValidate() {
			const token = getToken();
			if (!token) {
				this.gotologin();
				return;
			}
			try {
				const res = await request('/api/account/validate')
					.auth(token)
					.get()
				if (res.status == 200) {
					const { status, ...info } = res.data;
					match(status)({
						[AuthStatus.Ok]:
							() => {
								runInAction(() => this.props.user!.updateUser({ login: true, ...info }))
								this.checkRole(info.role!);
							},
						[AuthStatus.TokenExpired]:
							() => {
								removeToken();
								message.warn(Tips.TokenExpires, () => {
									this.gotologin();
								})
							},
						['_']:
							() => message.error(Tips.UnknownError)
					})
				}
				else {
					throw Error(res.statusText);
				}
			} catch (error) {
				const status = parseStatus(error);
				if (status) {
					match(status)({
						401: () => this.gotologin(),
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
