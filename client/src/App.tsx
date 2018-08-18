import { Provider } from "mobx-react";
import { ServicesStore } from "./services";
import * as React from "react";
import { renderRouter } from "./utils/core";
import 'antd/dist/antd.css';
import './assets/iconfont/iconfont.css'
import { indexRoutes, indexRedirect } from "./routes";
import { BrowserRouter } from "react-router-dom";
export default class App extends React.PureComponent {
	render() {
		return <Provider {...ServicesStore}>
			<BrowserRouter basename='/'>
				{renderRouter(indexRoutes, indexRedirect)}
			</BrowserRouter>
		</Provider>
	}
}
