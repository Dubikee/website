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
		showLoadingMore: true,
	}
	render() {
		const { loading, loadingMore, showLoadingMore } = this.state;
		const loadMore = showLoadingMore ? (
			<div style={{ textAlign: 'center', marginTop: 12, height: 32, lineHeight: '32px' }}>
				{loadingMore && <Spin />}
				{!loadingMore && <Button onClick={this.props.onLoadMore}>loading more</Button>}
			</div>
		) : null;
		let computeType = (str: string) => {
			let num = parseFloat(str);
			if (num < 60)
				return "error"

			else if (num < 70)
				return 'warning';
			else
				return 'success'
		}
		return <List
			className="demo-loadmore-list"
			loading={loading}
			itemLayout="horizontal"
			loadMore={loadMore}
			dataSource={this.props.data}
			renderItem={(item: ScoreInfo) => (
				<List.Item >
					<List.Item.Meta
						avatar={<Alert message={item.courseName} type={computeType(item.totalMark)} showIcon />}
					/>
					<Tag color='red'>{item.courseType}</Tag>


					<Tag color="red">{parseFloat(item.totalMark).toFixed(2)}</Tag>


					<Tag color="blue">{parseFloat(item.gpa).toFixed(2)}</Tag>

				</List.Item>
			)}
		/>
	}
}


export default ScoresList
