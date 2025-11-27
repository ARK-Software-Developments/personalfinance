
export class StorageSession {

    public static setItem(key: string, value: string): void {
        window.sessionStorage.setItem(key, value);
    }
    public static getItem(key: string): string | null {
        return window.sessionStorage.getItem(key);
    }
    public static removeItem(key: string): void {
        window.sessionStorage.removeItem(key);
    }   
    public static existsItem(key: string): boolean {
        return window.sessionStorage.getItem(key) !== null;
    }

};