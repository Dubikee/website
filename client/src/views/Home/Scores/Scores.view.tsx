import * as React from 'react'
import './Scores.view.less'
import { inject, observer } from 'mobx-react';
import { RouteComponentProps } from 'react-router';
import RinkCard from '../../../components/RinkCard/RinkCard';
import { Tips } from '../../../common/config/Tips';
import ScoresList from '../../../components/ScoresList/ScoresList';
import { WhutStudent } from '../../../common/stores/WhutStudent';
import { nullable, getToken, match } from '../../../utils';
import { request } from '../../../utils/request';
import { message, Tabs } from 'antd';
import { WhutStatus } from '../../../common/models/WhutStatus';

interface IScoresViewPorps extends RouteComponentProps<any> {
	student: WhutStudent | nullable
}

@inject('student')
@observer
class ScoresView extends React.Component<IScoresViewPorps>{
	state = {
		loading: false
	}
	componentWillMount() {
		let { rink, scores } = this.props.student!
		if (!rink || !scores)
			this.loadScores()
	}
	async loadScores() {
		try {
			let { status, data } = await request('/api/whut/scoresrink')
				.auth(getToken()!)
				.get()
			const ok = match({
				[WhutStatus.Ok]:
					() => {
						let { rink, scores } = data;
						let { student } = this.props;
						if (rink) student!.setRink(rink);
						if (scores) student!.setScores(scores);
						this.setState({ loadingRink: false });
					}
			});
			match({
				200: () => ok(data.status),
				401: () => message.warn(Tips.TokenExpires),
				'_': () => message.error(Tips.ServerFailure)
			})(status)
		} catch (error) {
			console.log(error)
			message.error(Tips.NetworkError);
		}
	}
	render() {
		let { rink, scores } = this.props.student!;
		let { loading } = this.state;
		return <div className="scores-view">
			<Tabs defaultActiveKey="2" >
				<Tabs.TabPane tab="绩点排名" key="1">
					<RinkCard loading={loading} data={rink} />
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
