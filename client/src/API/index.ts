import { LoginModel } from "./models/LoginModel";
import { UserInfoModel } from "./models/ValidateModel";
import { TableModel } from "./models/TableModel";
import { ScoresModel } from "./models/ScoresModel";
import { RinkModel } from "./models/RinkModel";
import { RegisterModel } from "./models/RegisterModel";
import { BaseModel } from "./models/BaseModel";
import { AllUserModel } from "./models/AllUserModel";

export interface API {
	"/api/account/login": LoginModel;
	"/api/account/validate": UserInfoModel;
	"/api/account/register": RegisterModel;
	"/api/account/updateinfo": BaseModel;
	"/api/account/updatepwd": BaseModel;
	"/api/admin/finduser": UserInfoModel;
	"/api/admin/allusers": AllUserModel;
	"/api/admin/deleteuser": BaseModel;
	"/api/admin/adduser": BaseModel;
	"/api/admin/edituser": BaseModel;
	"/api/whut/validate": BaseModel;
	"/api/whut/table": TableModel;
	"/api/whut/scores": ScoresModel;
	"/api/whut/rink": RinkModel;
}



