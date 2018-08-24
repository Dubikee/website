import { LoginModel } from "./LoginModel";
import { ValidateModel } from "./ValidateModel";
import { TableModel } from "./TableModel";
import { ScoresRinkModel } from "./ScoresRinkModel";

export interface API{
    '/api/account/login':LoginModel
    '/api/account/validate':ValidateModel
    '/api/whut/table':TableModel
    '/api/whut/updatetable':TableModel
    '/api/whut/scoresrink':ScoresRinkModel
    '/api/whut/updatescoresrink':ScoresRinkModel
}
