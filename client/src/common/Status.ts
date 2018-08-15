/**
 * 与服务端一致
 *
 * @export
 * @enum {number}
 */
export enum AuthStatus {
	UnknownError,
	Ok,
	InputIllegal,
	NotAllowed,
	TokenExpired,
	UidIllegal,
	UIdNotFind,
	UidHasExist,
	PasswordWrong,
	PasswordIllegal,
}

/**
 *
 *
 * @export
 * @enum {number}
 */
export enum WhutStatus {
	UnknownError,
	Ok,
	PwdWrong,
	StudentNotFind,
	InputIllegal,
	UserNotFind,
	WhutServerCrashed,
	CreateStudent
}
