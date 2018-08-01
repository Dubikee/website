import * as React from "react";
import { Provider } from "mobx-react";
import { ServicesStore } from "./services";
import { PComponent, renderRouter } from "./utils/core";
import 'antd/dist/antd.css';
import './assets/iconfont/iconfont.css'
import { routes, redirect } from "./routes";
export default class App extends PComponent {
    render() {
        return <Provider {...ServicesStore}>
            {/* <BrowserRouter basename="/">
                <Switch>
                    <Route path={'/'} component={HomeView} exact/>
                    <Route path={'/home'} component={HomeView}/>
                    <Route path={'/test/:id'} component={TestView}/>
                    <Redirect from={'/test'} to={'/home'}/>
                    <Redirect to={'/home'}/>
                </Switch>
            </BrowserRouter> */}
            {renderRouter(routes, redirect)}
        </Provider>
    }
}
