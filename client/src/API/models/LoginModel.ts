import { nullable } from "../../utils";
import { UserInfoModel } from "./ValidateModel";
export class LoginModel extends UserInfoModel {
	jwt: string | nullable;
}
