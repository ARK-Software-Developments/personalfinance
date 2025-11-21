"use strict";

import { CustomError } from "./interfaces/customError";

/**
 *
 * @param {*} res
 * @param {*} errors
 */
function errorResponse(errors: any) {

    let customError: CustomError = null as any;
    console.log("errorResponse:", errors);
    customError.code = errors.code || "INTERNAL_SERVER_ERROR";
    customError.message = errors.message || "Ha ocurrido un error interno en el servidor";
    customError.status = errors.status || 500;
    customError.details = errors.details || null;
    customError.source = errors.source || null; 
    return customError;
}

module.exports = {
    errorResponse
};