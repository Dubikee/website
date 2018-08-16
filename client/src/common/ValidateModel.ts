import { nullable } from "../utils/core";
import { AuthStatus } from "./AuthStatus";
export class ValidateModel {
	status: AuthStatus;
	uid: string | nullable;
	name: string | nullable;
	phone: string | nullable;
	email: string | nullable;
	role: string | nullable;
}
