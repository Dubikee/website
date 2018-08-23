import { AuthStatus } from "../models/AuthStatus";
import { nullable } from "../../utils";

export class ValidateModel {
	status: AuthStatus;
	uid: string | nullable;
	name: string | nullable;
	phone: string | nullable;
	email: string | nullable;
	role: string | nullable;
}
