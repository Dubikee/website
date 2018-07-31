import registerServiceWorker from "./registerServiceWorker";
import * as React from "react";
import * as ReactDOM from "react-dom";
export let PComponent = React.PureComponent;
export let bootstrap = () => {
  return new Bootstrap();
};
export interface IBootstrap {
  with(App: React.ComponentClass): IBootstrap;
  does(work: () => Promise<void> | void): IBootstrap;
  mount(selector: string): IBootstrap;
  start(): void;
}

class Bootstrap implements IBootstrap {
  private works: Array<() => Promise<void> | void> = [registerServiceWorker];
  private App: React.ComponentClass;
  private ele: Element;

  with(App: React.ComponentClass) {
    this.App = App;
    return this;
  }

  does(work: () => Promise<void> | void) {
    this.works.push(work);
    return this;
  }
  mount(selector: string) {
    let e = document.querySelector(selector);
    if (e) this.ele = e;
    else throw Error("找不到挂载点");
    return this;
  }
  start() {
    if (!this.ele || !this.App) throw Error("根组件与挂载点不可为空");
    let { works, App, ele } = this;
    ReactDOM.render(<App />, ele);
    works.forEach(f => f());
  }
}
