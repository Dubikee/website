import * as React from 'react'
import { Layout, Menu, Icon, Dropdown } from 'antd';
import "./Home.layout.less"
import { Link, RouteComponentProps, withRouter } from 'react-router-dom';
import { inject, observer } from 'mobx-react';
import { User } from '../../../common/User';
import { nullable, renderRouter } from '../../../utils/core';
import { homeRoutes, homeRedirect } from '../../../routes/home';
import AdminOnly from '../../../containers/AdminOnly/AdminOnly';

const menu = (
	<Menu>
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
);

interface IHomeLayoutProps extends RouteComponentProps<{}> {
	user: User | nullable
}
@inject('user')
@observer
class HomeLayout extends React.Component<IHomeLayoutProps> {
	state = {
		collapsed: false
	}
	render() {
		let { name } = this.props.user!;
		return <Layout className="home-layout">
			<Layout.Header className="header">
				<div className="logo" />
				<div className="menu">
					<div className="list">
						<p className="list-selected"><Link to="/home">首页</Link></p>
						<p><Link to="/index">XXXX</Link></p>
						<p><Link to="/index">XXXX</Link></p>
					</div>
				</div>
				<div className='info'>
					<Dropdown className="drop-menu" overlay={menu}>
						<Link to="#">
							<Icon type='user' />
							<span className='name'>{name}</span>
							<Icon type="down" />
						</Link>
					</Dropdown>
				</div>
			</Layout.Header>
			<Layout>
				<Layout.Sider trigger={null}
					collapsible
					collapsed={this.state.collapsed}
					width={200}
					style={{ background: '#fff' }}>
					<Menu
						mode="inline"
						defaultSelectedKeys={['/home/index']}
						defaultOpenKeys={['whut']}
						onSelect={({ key }) => this.handleClick(key)}
						style={{ height: '100%', borderRight: 0 }}
					>
						<Menu.SubMenu
							key="whut"
							title={<span><Icon type="home" /><span>教务处</span></span>}>
							<Menu.Item key="/home/index">课表</Menu.Item>
							<Menu.Item key="/home/xk">选课</Menu.Item>
							<Menu.Item key="/home/pj">评教</Menu.Item>
							<Menu.Item key="/home/scores">成绩</Menu.Item>
						</Menu.SubMenu>
						< Menu.SubMenu key="sub2" title={<span><Icon type="laptop" /><span>服务器</span></span>}>
							<Menu.Item key="5">option5</Menu.Item>
							<Menu.Item key="6">option6</Menu.Item>
							<Menu.Item key="7">option7</Menu.Item>
							<Menu.Item key="8">option8</Menu.Item>
						</Menu.SubMenu>
						<Menu.SubMenu key="sub3" title={<span><Icon type="notification" /><span>定时任务</span></span>}>
							<Menu.Item key="9">option9</Menu.Item>
							<Menu.Item key="10">option10</Menu.Item>
							<Menu.Item key="11">option11</Menu.Item>
							<Menu.Item key="12">option12</Menu.Item>
						</Menu.SubMenu>
						<div style={{
							textAlign: "center",
							marginTop: 10,
							backgroundColor: "#e1e1e1"
						}}>
							<Icon
								className="trigger"
								type={this.state.collapsed ? 'menu-unfold' : 'menu-fold'}
								onClick={() => this.setState({
									collapsed: !this.state.collapsed
								})}
							/>
						</div>
					</Menu>
				</Layout.Sider>
				<Layout className="right">
					<Layout.Content style={{ backgroundColor: '#fff', minHeight: 630 }}>
						{renderRouter(homeRoutes, homeRedirect)}
					</Layout.Content>
				</Layout>
			</Layout>
			<Layout.Footer style={{ textAlign: 'center' }}>
				Dubikee ©2018 Created by Dubikee
    		</Layout.Footer>
		</Layout>
	}
	handleClick(key: string) {
		this.props.history.push(key)
	}
}

export default AdminOnly(withRouter(HomeLayout))
