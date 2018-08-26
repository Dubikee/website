import * as React from 'react'
import { Layout, Menu, Icon } from 'antd';
import "./Home.less"
import { RouteComponentProps, withRouter } from 'react-router-dom';
import { renderSwitch } from '../../utils';
import { home } from '../../routes';
import AppLayout from '../../containers/AppLayout/App.Layout';
import { adminRequired } from '../../containers/Auth/Auth';

class Home extends React.Component<RouteComponentProps<{}>> {
	state = {
		collapsed: false
	}
	render() {
		return <Layout>
			<Layout.Sider trigger={null}
				collapsed={this.state.collapsed}
				width={200}
				style={{ background: '#fff' }}
				collapsible
			>
				<Menu
					mode="inline"
					defaultSelectedKeys={['/home/index']}
					defaultOpenKeys={['whut']}
					onSelect={({ key }) => this.handleClick(key)}
					style={{ height: '100%', borderRight: 0 }}
				>
					<Menu.SubMenu key="whut" title={<span><Icon type="home" /><span>教务处</span></span>}>
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
							className="sider-trigger"
							type={this.state.collapsed ? 'menu-unfold' : 'menu-fold'}
							onClick={() => this.setState({
								collapsed: !this.state.collapsed
							})}
						/>
					</div>
				</Menu>
			</Layout.Sider>
			<Layout className="content-wrapper">
				<Layout.Content style={{ backgroundColor: '#fff', minHeight: 630 }}>
					{renderSwitch(home)}
				</Layout.Content>
			</Layout>
		</Layout>
	}
	handleClick(key: string) {
		this.props.history.push(key)
	}
}

export default adminRequired(AppLayout(withRouter(Home)))
