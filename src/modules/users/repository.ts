import mysql from 'mysql2/promise';

// Logs de debbug
const debug = require('debug')('tablero-api:students');
// Schema de distrito
const Pool = require('../../db/schema/database');

async function login(body: any) {
    let result: any = null;
    try {
        console.log("login in repository");

        //let poolMysqlServer = await Pool();
        const pool = mysql.createConnection({
              host: 'localhost',
              user: 'desarrollo',
              password: 'Desarrollo2025',
              database: 'personalfinance',
              port: 3306,
              connectionLimit: 10
            });

            console.log("end in createConnection");
        result = await (await pool).query('SELECT * FROM users WHERE nick = ? AND password = ?;', [body.nick, body.password]);
    } catch (err) {
        console.error(new Date(Date.now()).toISOString() + '[login] ERROR!', err);
    }
    return result;
};

/*
async function getAll(filters: any) {
    let result: any = null;
    try {
        let poolMysqlServer = await Pool();
        result = await poolMysqlServer.execute('SELECT * FROM users;');
    } catch (err) {
        console.error(new Date(Date.now()).toISOString() + '[getAll] ERROR!', err);
    }
    return result;
};




function save(values) {
    return new Promise((resolve, reject) => {

        schema
            .create({"nombre": values.nombre,"id": values.id})
            .then(doc => {
                resolve(doc);
            })
            .catch(err => {
                reject(err);
            });
    });
}

function saveCadenas(values) {
    return new Promise((resolve, reject) => {
        schema
            .replaceOne({'nombre': values.nombre, 'id': values.id},
                {
                    'nombre': values.nombre, 
                    'id': values.id
                },
                {upsert: true})
            .then(doc => {
                resolve(doc);
            })
            .catch(err => {
                reject(err);
            });
    });
}

function update(values) {
    return new Promise((resolve, reject) => {
        schema
            .updateOne({"id": values.id}, values)
            .then(doc => {
                resolve(doc);
            })
            .catch(err => {
                reject(err);
            });
    });
}

function remove(values) {

    return new Promise((resolve, reject) => {
        schema
            .deleteMany({"id": values.id}, (err, result) => {
                if (err) {
                    console.error(new Date(Date.now()).toISOString() + err);
                }
                resolve(result);
            })
            .catch(err => {
                reject(err);
            });
    });
}
*/



module.exports = {
    login,
};