import { PComponent, React } from "../../utils/core";
import './Main.layout.less';
export default function MainLayout(View: any) {
	return class MainLayout extends PComponent {
		render() {
			return <div className="main-layout">
				<View></View>
			</div>
		}
	}
}
