
export interface ResponseData {
	status: HttpResult;
}


/**
 * 与服务端一致
 *
 * @export
 * @enum {number}
 */
export enum HttpResult {

	/// <summary>
	/// 0~ ，Ok
	/// </summary>
	Ok = 0,

	/// <summary>
	/// 1~
	/// 请求参数错误
	/// </summary>
	ParamsIsEmpty = 100,

	/// <summary>
	/// 2~
	/// 身份验证与权限错误
	/// </summary>
	NotAllowed = 200,
	TokenExpired = 201,

	/// <summary>
	/// 3~
	/// Uid错误
	/// </summary>
	UidTooShort = 300,
	UidIsNotNumbers = 301,
	UIdNotFind = 302,
	UidHasExist = 303,

	/// <summary>
	/// 4~
	/// 密码错误
	/// </summary>
	PasswordWrong = 400,
	PasswordTooShort = 401,
	PasswordNoNumbers = 402,
	PasswordNoLetters = 403,
	NewPasswordTooShort = 404,
	NewPasswordNoNumbers = 405,
	NewPasswordNoLetters = 406,

	/// <summary>
	/// 9~
	/// 其他错误
	/// </summary>
	UnknownError = 900,
}
