export interface CustomError {
    status: number;
    code: string;
    message: string;
    details?: any;    
    source?: string;
};