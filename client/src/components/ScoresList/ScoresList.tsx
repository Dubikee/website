import * as React from 'react';
import './ScoresList.less';
import { ScoreInfo } from '../../common/ScoreInfo';
import { List, Button, Spin, Tag, Alert } from 'antd';
import { nullable } from '../../utils/core';


interface IScoresListPorps {
	onLoadMore: () => void;
	data: ScoreInfo[] | nullable
}

class ScoresList extends React.PureComponent<IScoresListPorps, any> {
	state = {
		loading: false,
		loadingMore: false,
	}
	loadMore() {
		return <div style={{ textAlign: 'center', marginTop: 12, height: 32, lineHeight: '32px' }}>
			{this.state.loadingMore ? <Spin /> : <Button onClick={this.props.onLoadMore}>loading more</Button>}
		</div>
	}
	renderIsRetrain(info: ScoreInfo) {
		return info.isRetrain ? <div style={{ padding: '0 5px', textAlign: 'center' }}>
			<Tag color='red'>重修课</Tag>
		</div> : null

	}
	renderCourseType(info: ScoreInfo) {
		return <div style={{ padding: '0 5px', textAlign: 'center' }}>
			<Tag color={info.courseType == '必修课' ? 'orange' : 'green'}>{info.courseType}</Tag>
		</div>
	}
	renderMark(info: ScoreInfo) {
		let color: string;
		const mark = parseFloat(info.totalMark);
		if (mark < 60)
			color = 'red';
		else if (mark < 70)
			color = 'gold';
		else
			color = 'green'
		return < div style={{ padding: '0 5px', textAlign: 'center' }}>
			<Tag color={color}>{mark.toFixed(2)}</Tag>
		</div >
	}
	renderGpa(info: ScoreInfo) {
		let color: string;
		const mark = parseFloat(info.totalMark);
		if (mark < 60)
			color = 'red';
		else if (mark < 70)
			color = 'gold';
		else
			color = 'green'
		return <div style={{ padding: '0 5px', textAlign: 'center' }}>
			<Tag color={color}>{parseFloat(info.gpa).toFixed(2)}</Tag>
		</div>
	}
	renderMeta(info: ScoreInfo) {
		let type: 'success' | 'info' | 'warning' | 'error';
		const mark = parseFloat(info.totalMark);
		if (mark < 60)
			type = 'error';
		else if (mark < 70)
			type = 'warning';
		else
			type = 'success'
		return <List.Item.Meta
			avatar={<Alert message={info.courseName} type={type} showIcon />}
		/>
	}
	render() {
		return <List
			loading={this.state.loading}
			itemLayout="horizontal"
			loadMore={this.loadMore}
			dataSource={this.props.data}
			renderItem={(item: ScoreInfo) => (
				<List.Item >
					{this.renderMeta(item)}
					{this.renderIsRetrain(item)}
					{this.renderCourseType(item)}
					{this.renderMark(item)}
					{this.renderGpa(item)}
				</List.Item>
			)}
		/>
	}
}


export default ScoresList
