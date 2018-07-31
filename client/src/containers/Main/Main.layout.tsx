import { PComponent } from "../../utils/core";
import * as React from "react";
import { NavLink } from "react-router-dom";
import './Main.layout.less';
export default function MainLayout(View: any) {
    return class MainLayout extends PComponent {
        render() {
            return <div className='main-layout'>
                <div className={'nav'}>
                    <span>
                        <NavLink to={'/home'}>Home</NavLink>
                        <br/>
                        <NavLink to={'/test/2'}>Test</NavLink>
                    </span>
                </div>
                <View />
            </div>
        }
    }
}