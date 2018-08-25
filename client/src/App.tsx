import { Provider } from "mobx-react";
import { ServicesStore } from "./services";
import * as React from "react";
import { renderSwitch } from "./utils";
import 'antd/dist/antd.css';
import './assets/iconfont/iconfont.css'
import { BrowserRouter } from "react-router-dom";
import { index } from "./routes";
export default class App extends React.PureComponent {
	render() {
		return <Provider {...ServicesStore}>
			<BrowserRouter basename='/'>
				{renderSwitch(index)}
			</BrowserRouter>
		</Provider>
	}
}
