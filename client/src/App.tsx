import * as React from "react";
import {Provider} from "mobx-react";
import {ServicesStore} from "./services";
import {BrowserRouter, Redirect, Route, Switch} from "react-router-dom";
import {PComponent} from "./utils/core";
import HomeView from "./views/Home/Home.view";
import TestView from "./views/Test/Test.view";
import 'antd/dist/antd.css';
import './assets/iconfont/iconfont.css'
export default class App extends PComponent {
    render() {
        return <Provider {...ServicesStore}>
            <BrowserRouter basename="/">
                <Switch>
                    <Route path={'/'} component={HomeView} exact/>
                    <Route path={'/home'} component={HomeView}/>
                    <Route path={'/test/:id'} component={TestView}/>
                    <Redirect from={'/test'} to={'/home'}/>
                    <Redirect to={'/home'}/>
                </Switch>
            </BrowserRouter>
        </Provider>
    }
}
