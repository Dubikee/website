import { nullable } from "../utils/core";
import { AuthStatus } from "./Status";

export interface IValidateModel {
	status: AuthStatus;
	uid: string | nullable;
	name: string | nullable;
	phone: string | nullable;
	email: string | nullable;
	role: string | nullable;
}
