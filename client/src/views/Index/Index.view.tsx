import * as React from 'react'
import { PComponent } from '../../utils/core';
import { Layout, Menu, Icon } from 'antd';
import "./Index.view.less"
import { Link } from 'react-router-dom';

const { SubMenu } = Menu;
const { Header, Content, Sider, Footer } = Layout;

export class IndexView extends PComponent {
	state = {
		collapsed: false
	}
	render() {
		return <Layout className="index-view">
			<Header className="header">
				<div className="logo" />
				<div className="menu">
					<div className="list">
						<p className="list-selected"><Link to="/home">首页</Link></p>
						<p><Link to="/index">首页2</Link></p>
						<p><Link to="/index">首页3</Link></p>
					</div>
				</div>
				<div className='info'>
					<p>用户:罗坤</p>
				</div>
			</Header>
			<Layout>
				<Sider trigger={null}
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
						<SubMenu
							key="sub1"
							title={<span><Icon type="user" /><span>教务处</span></span>}>
							<Menu.Item key="1">课表</Menu.Item>
							<Menu.Item key="2">选课</Menu.Item>
							<Menu.Item key="3">评教</Menu.Item>
							<Menu.Item key="4">成绩</Menu.Item>
						</SubMenu>
						<SubMenu key="sub2" title={<span><Icon type="laptop" /><span>服务器</span></span>}>
							<Menu.Item key="5">option5</Menu.Item>
							<Menu.Item key="6">option6</Menu.Item>
							<Menu.Item key="7">option7</Menu.Item>
							<Menu.Item key="8">option8</Menu.Item>
						</SubMenu>
						<SubMenu key="sub3" title={<span><Icon type="notification" /><span>定时任务</span></span>}>
							<Menu.Item key="9">option9</Menu.Item>
							<Menu.Item key="10">option10</Menu.Item>
							<Menu.Item key="11">option11</Menu.Item>
							<Menu.Item key="12">option12</Menu.Item>
						</SubMenu>
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
				</Sider>
				<Layout className="right">
					<Content style={{ backgroundColor: '#fff', minHeight: 630 }}>
						Content
					</Content>
				</Layout>
			</Layout>
			<Footer style={{ textAlign: 'center' }}>
				Dubikee ©2018 Created by Dubikee
    		</Footer>
		</Layout>
	}
}
