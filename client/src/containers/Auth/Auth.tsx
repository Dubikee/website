import * as React from "react";
import { inject, observer } from "mobx-react";
import {
	getToken,
	removeToken,
	nullable,
	isRole} from "../../utils";
import { runInAction } from "mobx";
import { message, Spin } from "antd";
import { RouteComponentProps, withRouter } from "react-router";
import { Tips } from "../../common/config/Tips";
import { request } from "../../API/request";
import { User } from "../../common/stores/User";

interface IAdimOnlyPorps extends RouteComponentProps<any> {
	user: User | nullable;
}

const auth = (requriedRole: "admin" | "vistor" | "master") => (View: any) => {
	@inject("user")
	@observer
	class AdminOnly extends React.Component<IAdimOnlyPorps> {
		state = {
			finished: false
		};
		componentWillMount() {
			const { login, role } = this.props.user!;
			if (login) this.checkRole(role);
			else this.requestValidate();
		}
		checkRole(userRole: string) {
			isRole(userRole, requriedRole)
				? this.setState({ finished: true })
				: message.warn(Tips.PermissionDenied, () => this.gotologin());
		}
		gotologin() {
			this.props.history.push("/login", {
				from: this.props.location.pathname
			});
		}
		async requestValidate() {
			const token = getToken();
			if (!token) {
				this.gotologin();
				return;
			}
			await request("/api/account/validate")
				.auth(token)
				.on({
					ok: dat => {
						const { status, ...info } = dat;
						runInAction(() =>
							this.props.user!.updateUser({
								login: true,
								...info
							})
						);
						this.checkRole(info.role!);
					},
					tokenExpired: () => {
						removeToken();
						message.warn(Tips.TokenExpires, () => {
							this.gotologin();
						});
					},
					unauthorized401: () => {
						this.gotologin();
					},
					forbidden403: () => {
						message.error(Tips.Locked);
					},
					unknownError: () => {
						message.error(Tips.UnknownError);
					}
				})
				.get();
		}

		render() {
			return this.state.finished ? (
				<View />
			) : (
				<div
					style={{
						textAlign: "center",
						paddingTop: "100px",
						backgroundColor: "#fff"
					}}
				>
					<Spin size="large" />
				</div>
			);
		}
	}
	return withRouter(AdminOnly);
};

const vistorRequired = auth("vistor");
const adminRequired = auth("admin");
const masterRequired = auth("master");

export { adminRequired, masterRequired, vistorRequired };
