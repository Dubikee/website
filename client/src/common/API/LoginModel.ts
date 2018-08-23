import { ValidateModel } from "./ValidateModel";
import { nullable } from "../../utils";
export class LoginModel extends ValidateModel {
	jwt: string | nullable;
}
