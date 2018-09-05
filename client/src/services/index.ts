import { User } from '../common/stores/User';
import { RinkStore } from '../common/stores/RinkStore';
import { ScoresStore } from '../common/stores/ScoresStore';
import { TableStore } from '../common/stores/TableStore';
export let ServicesStore = {
	user: new User(),
	rinkStore: new RinkStore(),
	scoresStore: new ScoresStore(),
	tableStore: new TableStore()
};

