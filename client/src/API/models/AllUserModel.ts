import { UserInfoModel } from "./ValidateModel";
import { BaseModel } from "./BaseModel";
import { nullable } from "../../utils";
export class AllUserModel extends BaseModel {
	users: UserInfoModel[] | nullable;
}