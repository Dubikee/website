import * as React from "react";
import { PComponent } from "../../utils/core";
import { HttpClient } from "../../services";
import { inject } from "mobx-react";
import { ServiceTypes } from "../../services";
import { withRouter, RouteComponentProps } from "react-router";
import MainLayout from "../../containers/Main/Main.layout";

interface ITestViewProps extends RouteComponentProps<{ id: number }> {
  httpClient?: HttpClient;
}

@inject(ServiceTypes.HttpClient)
class TestView extends PComponent<ITestViewProps> {
  render() {
    return (
      <div>
        Num={this.props.httpClient!.num}
        <br />
        Id={this.props.match.params.id}
        <br />
        Location={JSON.stringify(this.props.location)}
      </div>
    );
  }
}

export default MainLayout(withRouter(TestView));
