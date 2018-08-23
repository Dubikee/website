import * as React from 'react';
import './RinkCard.less';
import { nullable } from '../../utils';
import { Card, Row } from 'antd';
import { Rink } from "../../common/models/Rink";

interface IRinkCardPorps {
	data: Rink | nullable,
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
