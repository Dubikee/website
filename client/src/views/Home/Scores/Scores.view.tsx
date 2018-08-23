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
import { message, Tabs, Icon, Button, Row, Dropdown, Menu, Select } from 'antd';
import { WhutStatus } from '../../../common/models/WhutStatus';

interface IScoresViewPorps extends RouteComponentProps<any> {
	student: WhutStudent | nullable
}

@inject('student')
@observer
class ScoresView extends React.Component<IScoresViewPorps>{
	state = {
		loading: true,
		year: '*'
	}
	componentWillMount() {
		let { rink, scores } = this.props.student!
		if (!rink || scores.length == 0)
			this.loadScores()
		else
			this.setState({ loading: false })
	}
	async loadScores() {
		try {
			let { status, data } = await request('/api/whut/scoresrink')
				.auth(getToken()!)
				.get()
			this.setState({ loading: false })
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
			this.setState({ loading: false })
			console.log(error)
			message.error(Tips.NetworkError);
		}
	}
	async refresh() {
		this.setState({ loading: true });
		await this.loadScores()
		message.info(Tips.RefreshOk)
	}
	render() {
		let { rink, scores } = this.props.student!;
		let { loading, year } = this.state;
		const yearSet = new Set(scores.map(x => x.schoolYear));
		return <div className="scores-view">
			<Tabs defaultActiveKey="2" >
				<Tabs.TabPane tab="绩点排名" key="1">
					<RinkCard loading={loading} data={rink} />
				</Tabs.TabPane>
				<Tabs.TabPane tab="考试成绩" key="2">
					<Row style={{ marginLeft: 25, marginRight: 40, padding: '8px 0 8px 0' }}>
						<span style={{ fontSize: 16, fontWeight: 600, letterSpacing: 2 }}>学年：</span>
						<Select defaultValue={year} style={{ minWidth: 150, marginRight: 20 }} onChange={p => this.setState({ year: p })}>
							<Select.Option value="*">全部</Select.Option>
							{Array.from(yearSet).map((y, i) => <Select.Option key={i} value={y}>{y}</Select.Option>)}
						</Select>
						<Button style={{ float: 'right' }} type="dashed" onClick={() => this.refresh()} icon='reload'>刷新</Button>
					</Row>
					<div style={{ padding: '0 25px 0 25px' }}>
						<ScoresList year={year} loading={loading} data={scores} />
					</div>
				</Tabs.TabPane>
			</Tabs>
		</div >
	}
}

export default ScoresView
