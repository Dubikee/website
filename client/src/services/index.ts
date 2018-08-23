import { WhutStudent } from '../common/stores/WhutStudent';
import { User } from '../common/stores/User';
export let ServicesStore = {
	user: new User(),
	student: new WhutStudent()
};

export let ServiceNames = {
	user: 'user',
	student: 'student'
};
