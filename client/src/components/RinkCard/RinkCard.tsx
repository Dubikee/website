import * as React from 'react';
import './RinkCard.less';
import { GpaRinks } from '../../common/GpaRinks';
import { nullable } from '../../utils/core';
import { Card, Row } from 'antd';

interface IRinkCardPorps {
	data: GpaRinks | nullable,
	loading: boolean
}

class RinkCard extends React.PureComponent<IRinkCardPorps, any> {
	render() {
		let { data } = this.props;
		return <Card loading={this.props.loading} title="绩点排名">
			{data ? <Row>
				<p>pureGPA={data.pureGpa}</p>
				<p>totalGpa={data.totalGpa}</p>
				<p>classRink={data.classRink}</p>
				<p>gradeRink={data.gradeRink}</p>
			</Row> : null}
		</Card>

	}
}

export default RinkCard
