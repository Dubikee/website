import { nullable } from "../utils/core";
import { ValidateModel } from "./ValidateModel";
export class LoginModel extends ValidateModel {
	jwt: string | nullable;
}
