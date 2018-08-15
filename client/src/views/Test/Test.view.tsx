import * as React from "react";
import { withRouter, RouteComponentProps } from "react-router";
import MainLayout from "../../containers/Main/Main.layout";

interface ITestViewProps extends RouteComponentProps<{ id: number }> {
	// httpClient?: HttpClient;
}

class TestView extends React.PureComponent<ITestViewProps> {
	render() {
		return (
			<div>
				<br />
				Id={this.props.match.params.id}
				<br />
				Location={JSON.stringify(this.props.location)}
			</div>
		);
	}
}

export default MainLayout(withRouter(TestView));
