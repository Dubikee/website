import { nullable } from "../utils/core";
import { IValidateModel } from "./IValidateModel";

export interface ILoginModel extends IValidateModel {
	jwt: string | nullable;
}
