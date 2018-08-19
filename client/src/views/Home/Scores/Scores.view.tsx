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
import { Errors } from '../../../common/config/Errors';
import ScoresList from '../../../components/ScoresList/ScoresList';

interface IScoresViewPorps extends RouteComponentProps<never> {
	student: WhutStudent | nullable
}

@inject('student')
@observer
class ScoresView extends React.Component<IScoresViewPorps>{
	state = {
		loadingRink: true
	}
	componentWillMount() {
		let { rinks, scores } = this.props.student!
		if (!rinks || !scores)
			this.loadData()
	}
	async loadData() {
		try {
			let res = await request('/api/whut/scorerink')
				.auth(getToken()!)
				.get<ScoresModel>()
			switch (res.status) {
				case 200:
					break;
				case 401:
					message.error(Errors.TokenExpires)
					return;
				default:
					message.error(Errors.ServerFailure)
					return;
			}
			let student = this.props.student!;
			let { status, rinks, scores } = res.data;
			switch (status) {
				case WhutStatus.Ok:
					student.rinks = rinks;
					student.scores = scores;
					this.setState({ loadingRink: false });
					break;
			}
		} catch (error) {
			console.log(error)
			message.error(Errors.NetworkError);
		}
	}
	render() {
		return <div className="scores-view">
			<Tabs defaultActiveKey="2" >
				<Tabs.TabPane tab="排名" key="1">
					<RinkCard loading={this.state.loadingRink} data={this.props.student!.rinks} />
				</Tabs.TabPane>
				<Tabs.TabPane tab="绩点" key="2">
					<div style={{padding:'0 10px 0 10px'}}>
						<ScoresList onLoadMore={() => { }} data={this.props.student!.scores} />
					</div>
				</Tabs.TabPane>
			</Tabs>
		</div>
	}
}

export default ScoresView
