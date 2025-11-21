import mysql from 'mysql2/promise';
import db from "../../data/database.json";
import { CustomResponse } from '../../middlewares/interfaces/customResponse';
const helper = require("../../modules/entities/helper");

/**
 * Clase de Gestion de Entidades Financieras.
 */
export class EF {
    constructor() {
    }
    /**
     * Método para obtener todos los registros de la entidad.
     * @returns 
     */
    public async GetAll() {

        let status: number = 200;
        let customResponse: CustomResponse = {
            meta: {
                method: 'get',
                operation: '/entities'
            },
            data: null,
        };

        try {
            console.log("get in entities route");

            helper.obtenerEntidades()
                .then((data: any) => {
                    customResponse.data = data;
                    return { status, customResponse };
                })
                .catch((err: any) => {
                    console.error("catch in entities route");
                    customResponse.error = err;
                    status = 401;
                    return { status, customResponse };
                });
        } catch (error: any) {
            customResponse.error = error;
            status = 500;
            return { status, customResponse };
        }
    };
};