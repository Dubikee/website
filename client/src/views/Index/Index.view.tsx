import * as React from 'react'
import "./Index.view.less"
import AdminOnly from '../../containers/AdminOnly/AdminOnly';
import { inject } from 'mobx-react';
import { User } from '../../common/User';
import { nullable } from '../../utils/core';
import MainLayout from '../../containers/Main/Main.layout';
@inject('user')
class IndexView extends React.PureComponent<{ user: User | nullable }> {
	render() {
		return <div>
			Context
		</div>
	}
}

export default AdminOnly(MainLayout(IndexView))
