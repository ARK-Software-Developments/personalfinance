import { CustomError } from "./customError";
import { Meta } from "./meta";

export interface CustomResponse {
    meta?: Meta | null;
    data?: any | null;
    error?: CustomError | null;
};