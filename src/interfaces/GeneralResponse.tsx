import { Meta } from "./Meta";

export interface GeneralResponse {
    meta?: Meta;
    data?: any[];
    erros?: Error[];
};