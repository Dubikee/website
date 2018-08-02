import { Provider } from "mobx-react";
import { ServicesStore } from "./services";
import { PComponent, renderRouter,React } from "./utils/core";
import 'antd/dist/antd.css';
import './assets/iconfont/iconfont.css'
import { routes, redirect } from "./routes";
export default class App extends PComponent {
    render() {
        return <Provider {...ServicesStore}>
            {renderRouter(routes, redirect)}
        </Provider>
    }
}
