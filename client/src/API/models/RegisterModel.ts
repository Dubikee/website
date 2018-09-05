import {BaseModel} from "./BaseModel";
import { nullable } from "../../utils";
export class RegisterModel extends BaseModel {
	jwt: string | nullable;
}
