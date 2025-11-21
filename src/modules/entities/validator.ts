


export class User {

    public static validateNick(nick: string): boolean {
        const nickRegex = /^[a-zA-Z0-9_]{3,20}$/;
        return nickRegex.test(nick);
    }

    public static validatePassword(password: string): boolean {
        const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
        return passwordRegex.test(password);
    }

    public static validateMatch(data: any): boolean {        
        return data == null || data === '' || data.length == 0 ? false : true;
    }
};