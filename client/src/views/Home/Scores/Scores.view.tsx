import * as React from 'react'
import './Scores.view.less'
import { inject, observer } from 'mobx-react';
import { RouteComponentProps } from 'react-router';
import { WhutStudent } from '../../../common/stores/WhutStudent';
import { nullable, request, getToken } from '../../../utils/core';
import { ScoresModel } from '../../../common/ScoresModel';
import { message, Tabs } from 'antd';
import { WhutStatus } from '../../../common/models/WhutStatus';
import RinkCard from '../../../components/RinkCard/RinkCard';
import { Errors } from '../../../common/config/Errors';
import ScoresList from '../../../components/ScoresList/ScoresList';

interface IScoresViewPorps extends RouteComponentProps<any> {
	student: WhutStudent | nullable
}

@inject('student')
@observer
class ScoresView extends React.Component<IScoresViewPorps>{
	state = {
		loading: true
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
					if (rinks)
						student.rinks = rinks;
					if (scores)
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
		let { rinks, scores } = this.props.student!;
		let { loading } = this.state;
		return <div className="scores-view">
			<Tabs defaultActiveKey="2" >
				<Tabs.TabPane tab="绩点排名" key="1">
					<RinkCard loading={loading} data={rinks} />
				</Tabs.TabPane>
				<Tabs.TabPane tab="考试成绩" key="2">
					<div style={{ padding: '0 25px 0 25px' }}>
						<ScoresList loading={loading} loadingMore={false} onLoadMore={() => { }} data={scores} />
					</div>
				</Tabs.TabPane>
			</Tabs>
		</div>
	}
}

export default ScoresView
