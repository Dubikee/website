import * as React from 'react'
import './Scores.view.less'
import { inject, observer } from 'mobx-react';
import { RouteComponentProps } from 'react-router';
import RinkCard from '../../../components/RinkCard/RinkCard';
import { Tips } from '../../../common/config/Tips';
import ScoresList from '../../../components/ScoresList/ScoresList';
import { WhutStudent } from '../../../common/stores/WhutStudent';
import { nullable, getToken, match, removeToken, parseStatus } from '../../../utils';
import { request } from '../../../utils/request';
import { message, Tabs, Modal, Button, Row, Select } from 'antd';
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
			this.loadScores(true)
		else
			this.setState({ loading: false })
	}
	async loadScores(useServerCache: boolean) {
		try {
			const url = useServerCache ? '/api/whut/scoresrink' : "/api/whut/updatescoresrink"
			const res = await request(url)
				.auth(getToken()!)
				.get()
			this.setState({ loading: false })
			if (res.status == 200) {
				const { status, rink, scores } = res.data;
				match(status)({
					[WhutStatus.Ok]:
						() => {
							const { student } = this.props;
							if (rink)
								student!.setRink(rink);
							if (scores)
								student!.setScores(scores);
							this.setState({ loadingRink: false });
							message.info(Tips.Ok)
						},
					[WhutStatus.StudentNotFind]:
						() => {
							message.warn(Tips.NoStudent);
						},
					[WhutStatus.WhutServerCrashed]:
						() => {
							message.error(Tips.WhutServerCrashed);
						},
					['_']:
						() => {
							message.error(Tips.UnknownError);
						}
				});
			}
			else {
				throw Error(res.statusText);
			}
		} catch (error) {
			this.setState({ loading: false })
			const status = parseStatus(error);
			if (status) {
				match(status)({
					401: () => message.warn(Tips.TokenExpires, () => {
						removeToken();
						this.props.history.push('/login', { from: this.props.location.pathname });
					}),
					423: () => message.error(Tips.Locked),
					'_': () => message.error(Tips.ServerFailure)
				})
			}
			else {
				console.log(error);
				message.error(Tips.NetworkError);
			}
		}
	}
	refresh() {
		this.setState({ loading: true });
		Modal.confirm({
			title: '是否清空服务器缓存？',
			content: '清空缓存将对教务处重新发起请求',
			okText: '是',
			okType: 'danger',
			cancelText: '否',
			onOk: async () => await this.loadScores(false),
			onCancel: async () => await this.loadScores(true),
		});
	}
	gotologin() {
		this.props.history.push('/login', { from: this.props.location.pathname });
	}
	render() {
		let { rink, scores } = this.props.student!;
		let { loading, year } = this.state;
		const yearSet = new Set(scores.map(x => x.schoolYear));
		return <div style={{ marginBottom: 10 }}>
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
