import * as React from 'react'
import { Layout, Menu, Icon, Dropdown } from 'antd';
import "./Home.layout.less"
import { Link } from 'react-router-dom';
import { inject } from 'mobx-react';
import { User } from '../../../common/User';
import { nullable, renderRouter } from '../../../utils/core';
import { whutRoutes, whutRedirect } from '../../../routes/whut';
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

@inject('user') class HomeLayout extends React.PureComponent<{ user: User | nullable }> {
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
						<p><Link to="/index">首页2</Link></p>
						<p><Link to="/index">首页3</Link></p>
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
						defaultSelectedKeys={['1']}
						defaultOpenKeys={['sub1']}
						style={{ height: '100%', borderRight: 0 }}
					>
						<Menu.SubMenu
							key="sub1"
							title={<span><Icon type="home" /><span>教务处</span></span>}>
							<Menu.Item key="1">课表</Menu.Item>
							<Menu.Item key="2">选课</Menu.Item>
							<Menu.Item key="3">评教</Menu.Item>
							<Menu.Item key="4">成绩</Menu.Item>
						</ Menu.SubMenu>
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
						{renderRouter(whutRoutes, whutRedirect)}
					</Layout.Content>
				</Layout>
			</Layout>
			<Layout.Footer style={{ textAlign: 'center' }}>
				Dubikee ©2018 Created by Dubikee
    		</Layout.Footer>
		</Layout>
	}
}

export default AdminOnly(HomeLayout)
