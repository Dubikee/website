import * as React from "react";
import { Layout, Menu, Icon, Dropdown } from "antd";
import { Link, RouteComponentProps, withRouter } from "react-router-dom";
import { inject, observer } from "mobx-react";
import { User } from "../../common/stores/User";
import { nullable } from "../../utils";
import "./App.layout.less";

interface IAppLayoutProps extends RouteComponentProps<{}> {
	user: User | nullable;
}

export default (View: any) => {
	@inject("user")
	@observer
	class AppLayout extends React.Component<IAppLayoutProps> {
		render() {
			const { login, name } = this.props.user!;
			const url = this.props.location.pathname;
			const menu = (
				<Menu>
					<Menu.Item>
						<Link to="/account/index">账号管理</Link>
					</Menu.Item>
					<Menu.Item>
						<Link to="/logout">注销账号</Link>
					</Menu.Item>
					<Menu.Item>
						<Link to={{ pathname: "/login", state: { from: url } }}>
							重新登陆
						</Link>
					</Menu.Item>
				</Menu>
			);
			return (
				<Layout className="app-layout">
					<Layout.Header className="cp-header">
						<div className="header-logo" />
						{login ? (
							<div className="header-userinfo">
								<Dropdown className="drop-menu" overlay={menu}>
									<Link to="/account/index">
										<Icon type="user" />
										<span style={{ marginLeft: 10 }}>
											{name}
										</span>
										<Icon type="down" />
									</Link>
								</Dropdown>
							</div>
						) : null}
					</Layout.Header>
					<View />
					<Layout.Footer style={{ textAlign: "center" }}>
						Dubikee ©2018 Created by Dubikee
					</Layout.Footer>
				</Layout>
			);
		}
	}
	return withRouter(AppLayout);
};
