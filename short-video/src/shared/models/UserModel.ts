export interface IUserModel {
    username: string;
    fullName: string;
    token: string;
}

export interface Login {
    username: string | undefined;
    password: string | undefined;
}