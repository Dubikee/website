import { nullable } from "../../utils";
import { BaseModel } from "./BaseModel";

export class UserInfoModel extends BaseModel {
	uid: string | nullable;
	name: string | nullable;
	phone: string | nullable;
	email: string | nullable;
	role: string | nullable;
	whutId: string | nullable;
}
