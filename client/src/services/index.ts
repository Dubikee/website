import { User } from '../common/User';
import { WhutStudent } from '../common/WhutStudent';
export let ServicesStore = {
	user: new User(),
	student: new WhutStudent()
};

export let ServiceNames = {
	user: 'user',
	student: 'student'
};
