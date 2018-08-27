import * as React from 'react'
import { Layout, Menu, Icon, Dropdown } from 'antd';
import { Link, RouteComponentProps, withRouter } from 'react-router-dom';
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
		state = {
			selected: ''
		}
		componentWillMount() {
			const url = this.props.location.pathname;
			if (url.startsWith('/home')) {
				this.setSelected('home')
			}
			else if (url.startsWith('/account')) {
				this.setSelected('account')
			}
			else if (url.startsWith('/master')) {
				this.setSelected('master')
			}
		}
		setSelected(v: string) {
			const { selected } = this.state;
			if (selected !== v)
				this.setState({ selected: v })
		}
		handleClick(item: string) {
			if (item !== this.state.selected) {
				this.props.history.push(`/${item}/index`);
			}
		}
		render() {
			const { login, name } = this.props.user!;
			const { selected } = this.state;
			return <Layout className="app-layout">
				<Layout.Header className="header">
					<div className="header-logo" />
					<div className="header-menu">
						<div className="menu-list">
							<div onClick={() => this.handleClick('home')}
								className={selected === 'home' ? "menu-item item-selected" : 'menu-item'}>
								<p>首页</p>
							</div>
							<div onClick={() => this.handleClick('account')}
								className={selected === 'account' ? "menu-item item-selected" : 'menu-item'}>
								<p>账号</p>
							</div>
							<div onClick={() => this.handleClick('master')}
								className={selected === 'master' ? "menu-item item-selected" : 'menu-item'}>
								<p>管理</p>
							</div>
						</div>
					</div>
					{
						login ? <div className='header-userinfo'>
							<Dropdown className="drop-menu" overlay={menu}>
								<Link to="#">
									<Icon type='user' />
									<span style={{ marginLeft: 10 }}>{name}</span>
									<Icon type="down" />
								</Link>
							</Dropdown>
						</div> : null
					}
				</Layout.Header>
				<View />
				<Layout.Footer style={{ textAlign: 'center' }}>
					Dubikee ©2018 Created by Dubikee
    		</Layout.Footer>
			</Layout>
		}
	}
	return withRouter(AppLayout)
}
