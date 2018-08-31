import * as React from 'react';
import './ScoresList.less';
import { List, Tag, Alert, Popover } from 'antd';
import { Score } from "../../common/models/Score";

interface IScoresListPorps {
	loading: boolean,
	data: Score[],
	year: string
}

const getInfo = (totalMark: string) => {
	let color: 'green' | 'gold' | 'red' = 'red';
	let cmark: string = totalMark;
	let cgpa: string = totalMark;
	let ctype: 'success' | 'warning' | 'error' = 'error';
	let phColor: "#b7eb8f" | "#ffe58f" | "#ffa39e" = "#ffa39e"
	const numRegex = /^[.|0-9]+$/;
	if (numRegex.test(totalMark)) {
		const m = parseFloat(totalMark);
		if (m > 70) {
			ctype = 'success'
			color = 'green'
			phColor = "#b7eb8f"
		}
		else if (m > 60) {
			ctype = 'warning';
			color = 'gold';
			phColor = "#ffe58f"
		}
		cgpa = ((m - 50) / 10).toFixed(2);
	}
	else {
		switch (totalMark) {
			case '优秀':
				ctype = 'success';
				color = 'green';
				phColor = "#b7eb8f"
				break;
			case '良好':
				ctype = 'success';
				color = 'green';
				phColor = "#b7eb8f"
				break;
			case '合格':
				ctype = 'warning';
				color = 'gold';
				phColor = "#ffe58f"
				break;
			case '未评教':
				ctype = 'warning';
				color = 'gold';
				phColor = "#ffe58f"
				break;
		}
	}
	return {
		color, cmark, cgpa, ctype, phColor
	}
}

class ScoresList extends React.PureComponent<IScoresListPorps> {
	renderItem(item: Score) {
		const {
			totalMark,
			isRetrain,
			firstScore,
			courseCredit,
			schoolYear,
			bestScore,
			courseName,
			courseType
		} = item;
		const { cgpa, color, cmark, ctype, phColor } = getInfo(totalMark);
		const popover = <div>
			<p>成绩：{totalMark}</p>
			<p>绩点：{cgpa}</p>
			<p>学分：{courseCredit}</p>
			<p>学年：{schoolYear}</p>
			{isRetrain ? <p>是否重修：是</p> : null}
			{firstScore ? <p>初次成绩：{firstScore}</p> : null}
			{bestScore ? <p>最高成绩：{bestScore}</p> : null}
		</div>
		let phName = courseName;
		if (courseName.length > 10) {
			phName = courseName.slice(0, 8) + ' . . .';
		}
		return <List.Item className="scoreslist-item">
			<List.Item.Meta className="cp-meta" avatar={<Alert message={courseName} type={ctype} showIcon />} />
			<List.Item.Meta className="ph-meta" avatar={<Tag color={phColor}>{phName}</Tag>} />
			{
				isRetrain ? <div className="retrain">
					<Tag color='red'>重修课</Tag>
				</div> : null
			}
			<Popover content={popover} title={courseName}>
				<div className="mark">
					<Tag color={color}>{cmark}</Tag>
				</div >
			</Popover>
			<div className="gpa">
				<Tag color={color}>{cgpa}</Tag>
			</div>
			<div className="type">
				<Tag color={courseType.includes('必修') ? 'rgb(254, 147, 137)' : 'rgb(108, 203, 208)'}>{courseType}</Tag>
			</div>
			{/* Phone */}

		</List.Item>
	}
	render() {
		const { data, loading, year } = this.props;
		const filter = (scores: Score[]) => year === '*' ? scores : scores.filter(x => x.schoolYear === this.props.year);
		return <List className="scoreslist" loading={loading} dataSource={filter(data)} itemLayout="horizontal" renderItem={this.renderItem} />
	}
}

export default ScoresList
