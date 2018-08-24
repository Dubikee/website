import * as React from 'react'
import { Layout, Menu, Icon, Dropdown } from 'antd';
import { Link, RouteComponentProps } from 'react-router-dom';
import { inject, observer } from 'mobx-react';
import { User } from '../../common/stores/User';
import { nullable } from '../../utils';
import './App.layout.less'

const menu = <Menu>
	<Menu.Item>
		<Link to="#">账号管理</Link>
	</Menu.Item>
	<Menu.Item>
		<Link to="#">注销账号</Link>
	</Menu.Item>
	<Menu.Item>
		<Link to="#">重新登陆</Link>
	</Menu.Item>
</Menu>

interface IAppLayoutProps extends RouteComponentProps<{}> {
	user: User | nullable
}

export default (View: any) => {
	@inject('user')
	@observer
	class AppLayout extends React.Component<IAppLayoutProps> {
		render() {
			let { name } = this.props.user!;
			return <Layout className="app-layout">
				<Layout.Header className="header">
					<div className="header-logo" />
					<div className="header-menu">
						<div className="menu-list">
							<p className="list-selected"><Link to="/home">首页</Link></p>
							<p><Link to="/index">XXXX</Link></p>
							<p><Link to="/index">XXXX</Link></p>
						</div>
					</div>
					<div className='header-userinfo'>
						<Dropdown className="drop-menu" overlay={menu}>
							<Link to="#">
								<Icon type='user' />
								<span style={{ marginLeft: 10 }}>{name}</span>
								<Icon type="down" />
							</Link>
						</Dropdown>
					</div>
				</Layout.Header>
				<View />
				<Layout.Footer style={{ textAlign: 'center' }}>
					Dubikee ©2018 Created by Dubikee
    		</Layout.Footer>
			</Layout>
		}
	}
	return AppLayout
}
