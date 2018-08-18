import * as React from 'react'
import './Scores.view.less'
import { inject, observer } from 'mobx-react';
import { RouteComponentProps } from 'react-router';
import { WhutStudent } from '../../../common/WhutStudent';
import { nullable, request, getToken } from '../../../utils/core';
import { ScoresModel } from '../../../common/ScoresModel';
import { message, Tabs } from 'antd';
import { WhutStatus } from '../../../common/WhutStatus';
import RinkCard from '../../../components/RinkCard/RinkCard';

interface IScoresViewPorps extends RouteComponentProps<never> {
	student: WhutStudent | nullable
}

@inject('student')
@observer
class ScoresView extends React.PureComponent<IScoresViewPorps>{
	state = {
		loadingRink: true
	}
	componentWillMount() {
		let { rinks, scores } = this.props.student!
		if (!rinks || !scores)
			this.loadData()
	}
	loadData() {
		request('/api/whut/scorerink')
			.auth(getToken()!)
			.get<ScoresModel>()
			.then(({ status, data }) => {
				switch (status) {
					case 200:
						return data;
					default:
						throw Error();
				}
			}).then(({ status, rinks, scores }) => {
				let student = this.props.student!;
				switch (status) {
					case WhutStatus.Ok:
						student.rinks = rinks;
						student.scores = scores;
						this.setState({ loadingRink: false });
						break;
				}
			}).catch(err => {
				console.log(err)
				message.error("请求失败");
			})
	}
	render() {
		return <div className="scores-view">
			<Tabs defaultActiveKey="1" >
				<Tabs.TabPane tab="排名" key="1">
					<RinkCard loading={this.state.loadingRink} data={this.props.student!.rinks} />
				</Tabs.TabPane>
				<Tabs.TabPane tab="分数" key="2">Scores</Tabs.TabPane>
			</Tabs>
		</div>
	}
}

export default ScoresView
