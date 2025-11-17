import { CONN } from "./Mysql";
import db from "../../data/database.json"

/**
 * Clase de Gestion de Entidades Financieras.
 */
export class EF {
    cn: any;

    constructor() {
        this.cn = CONN();
    }
    /**
     * Método para obtener todos los registros de la entidad.
     * @returns 
     */
    public GetAll() {

        // Ejecutar consulta (ejemplo: obtener todos los usuarios de una tabla)
        //const [rows] = this.cn.execute("SELECT `id`,`entity`,`type` FROM `entities`;");
        // Cerrar la conexión
        //await this.cn.end();

        return db.entidades;
    };
};