import * as React from 'react';
import './ScoresList.less';
import { List, Button, Spin, Tag, Alert, Popover } from 'antd';
import { Score } from "../../common/models/Score";
import { nullable } from '../../utils';

interface IScoresListPorps {
	//onLoadMore: () => void;
	loading: boolean,
	// loadingMore: boolean,
	data: Score[],
	year: string
}

class ScoresList extends React.PureComponent<IScoresListPorps> {
	// loadMore() {
	// 	let { loadingMore, onLoadMore } = this.props;
	// 	return <div className='loadmore' style={{ textAlign: 'center', marginTop: 12, height: 32, lineHeight: '32px' }}>
	// 		{loadingMore ? <Spin /> : <Button onClick={onLoadMore}>loading more</Button>}
	// 	</div>
	// }
	renderItem(item: Score) {
		const { totalMark, isRetrain, firstScore, courseCredit, schoolYear, bestScore, courseName, courseType } = item;
		let type: 'success' | 'warning' | 'error';
		let color: string;
		let mark: string;
		let gpa: string;
		const ness = (/.+必修$/).test(courseType);
		if ((/^[.|0-9]+$/).test(totalMark)) {
			const m = parseFloat(totalMark);
			if (m < 60) {
				type = 'error';
				color = 'red';
			}
			else if (m < 70) {
				type = 'warning';
				color = 'gold';
			}
			else {
				type = 'success'
				color = 'green'
			}
			mark = m.toFixed(2);
			gpa = ((m - 50) / 10).toFixed(2);
		}
		else {
			switch (totalMark) {
				case '优秀':
					type = 'success';
					color = 'green';
					break;
				case '良好':
					type = 'success';
					color = 'green';
					break;
				case '合格':
					type = 'warning';
					color = 'gold';
					break;
				case '未评教':
					type = 'warning';
					color = 'gold';
					break;
				default:
					type = 'success'
					color = 'green'
					break;
			}
			gpa = totalMark;
			mark = totalMark;
		}
		let content = <div>
			<p>成绩：{totalMark}</p>
			<p>绩点：{gpa}</p>
			<p>学分：{courseCredit}</p>
			<p>学年：{schoolYear}</p>
			{isRetrain ? <p>是否重修：是</p> : null}
			{firstScore ? <p>初次成绩：{firstScore}</p> : null}
			{bestScore ? <p>最高成绩：{bestScore}</p> : null}
		</div>

		return <List.Item>
			<List.Item.Meta avatar={<Alert message={courseName} type={type} showIcon />} />
			{
				isRetrain ? <div style={{ width: 70, textAlign: 'center' }}>
					<Tag color='red'>重修课</Tag>
				</div> : null
			}
			<Popover content={content} title={courseName}>
				< div style={{ width: 70, textAlign: 'center' }}>
					<Tag color={color}>{mark}</Tag>
				</div >
			</Popover>
			<div style={{ width: 70, textAlign: 'center' }}>
				<Tag color={color}>{gpa}</Tag>
			</div>
			<div style={{ width: 80, textAlign: 'center' }}>
				<Tag color={ness ? 'rgb(254, 147, 137)' : 'rgb(108, 203, 208)'}>{courseType}</Tag>
			</div>
		</List.Item>
	}
	filter(scores: Score[]) {
		if (this.props.year && this.props.year !== '*') {
			return scores.filter(x => x.schoolYear === this.props.year);
		}
		return scores;
	}
	render() {
		let { data, loading } = this.props;
		return <List
			loading={loading}
			dataSource={this.filter(data)}
			itemLayout="horizontal"
			// loadMore={this.loadMore()}
			renderItem={this.renderItem}
		/>
	}

}


export default ScoresList
