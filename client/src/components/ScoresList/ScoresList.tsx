import * as React from 'react';
import './ScoresList.less';
import { List, Button, Spin, Tag, Alert, Popover } from 'antd';
import { Score } from "../../common/models/Score";

interface IScoresListPorps {
	onLoadMore: () => void;
	loading: boolean,
	loadingMore: boolean,
	data: Score[]
}

class ScoresList extends React.PureComponent<IScoresListPorps> {
	loadMore() {
		let { loadingMore, onLoadMore } = this.props;
		return <div style={{ textAlign: 'center', marginTop: 12, height: 32, lineHeight: '32px' }}>
			{loadingMore ? <Spin /> : <Button onClick={onLoadMore}>loading more</Button>}
		</div>
	}
	renderItem(item: Score) {
		const { totalMark, isRetrain, gpa, firstScore, bestScore, courseName, courseType } = item;
		let type: 'success' | 'info' | 'warning' | 'error';
		let color: string;
		const mark = parseFloat(totalMark);
		if (mark < 60) {
			type = 'error';
			color = 'red';
		}
		else if (mark < 70) {
			type = 'warning';
			color = 'gold';
		}
		else {
			type = 'success'
			color = 'green'
		}
		let content = <div>
			<p>成绩:{totalMark}</p>
			<p>绩点:{gpa}</p>
			{isRetrain ? <p>是否重修:是</p> : null}
			{firstScore ? <p>初次成绩:{firstScore}</p> : null}
			{bestScore ? <p>最高成绩:{bestScore}</p> : null}
		</div>
		let avatar = <Popover content={content} title={courseName}>
			<Alert message={courseName} type={type} showIcon />
		</Popover>

		return <List.Item>
			<List.Item.Meta avatar={avatar} />
			{
				isRetrain ? <div style={{ padding: '0 5px', textAlign: 'center' }}>
					<Tag color='red'>重修课</Tag>
				</div> : null
			}
			< div style={{ padding: '0 5px', textAlign: 'center' }}>
				<Tag color={color}>{mark.toFixed(2)}</Tag>
			</div >
			<div style={{ padding: '0 5px', textAlign: 'center' }}>
				<Tag color={color}>{parseFloat(gpa).toFixed(2)}</Tag>
			</div>
			<div style={{ padding: '0 5px', textAlign: 'center' }}>
				<Tag color={courseType == '必修课' ? 'orange' : 'green'}>{courseType}</Tag>
			</div>
		</List.Item>
	}
	render() {
		let { data, loading } = this.props;
		return <List
			loading={loading}
			dataSource={data}
			itemLayout="horizontal"
			loadMore={this.loadMore}
			renderItem={this.renderItem}
		/>
	}

}


export default ScoresList
