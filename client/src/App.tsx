import { Provider } from "mobx-react";
import { ServicesStore } from "./services";
import * as React from "react";
import {  renderRouter } from "./utils/core";
import 'antd/dist/antd.css';
import './assets/iconfont/iconfont.css'
import { routes, redirect } from "./routes";
export default class App extends React.PureComponent {
	render() {
		return <Provider {...ServicesStore}>
			{renderRouter(routes, redirect)}
		</Provider>
	}
}
