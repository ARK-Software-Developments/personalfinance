"use strict";

import { CustomError } from "../../middlewares/interfaces/customError";
import { User } from "./validator";

// Configuracion de express
const express = require("express");

const repository = require("./repository");
const errorHandler  = require("../../middlewares/errorHandler");

/**
 * Método para loguearse a la aplicación.
 * @param req 
 * @returns 
 */
function ingreso(body: any) {
    return new Promise((resolve, reject) => {
        console.log("ingreso in helper");
        repository
            .login(body)
            .then((response: any) => {
                
                if(!User.validateNick(body.nick)){
                    throw new Error("Nick inválido");
                }

                if(!User.validateMatch(response[0])){
                    throw new Error("Usuario o contraseña incorrecta");
                }

                resolve(response[0]);
            })
            .catch((err: any) => {
                reject({
                    message: err.message || "No autorizado",
                    status: 401,
                    code: "UNAUTHORIZED",
                    source: "ingreso",
                });
            });
    });
};

/*
function obtenerUsuarios(req: any) {
    return new Promise((resolve, reject) => {
        repository
            .getAll(req)
            .then((response: unknown) => {
                resolve(response);
            })
            .catch((err: any) => {
                reject(err);
            });
    });
};
function insert(req) {

    return new Promise((resolve, reject) => {
        repository
            .save(
                req
            )
            .then(result => {
                resolve(result);
            })
            .catch(err => {
                reject(err);
            });
    })

}

function insertCadenas(req) {
    const cantidadNivelProductos = req.CADENAS.length;

    return new Promise((resolve, reject) => {
        req.CADENAS.forEach(function (y) {
            repository
                .saveCadenas(
                    y
                )
                .then(result => {
                    resolve(result);
                })
                .catch(err => {
                    reject(err);
                });
        })
    });
}

function update(req) {

    return new Promise((resolve, reject) => {
        repository
            .update(
                req
            )
            .then(result => {
                resolve(result);
            })
            .catch(err => {
                reject(err);
            });
    })

}

function remove(req) {
    return new Promise((resolve, reject) => {
        repository
            .remove(req)
            .then(doc => {
                resolve(doc);
            })
            .catch(err => {
                reject(err);
            });
    });
}

*/


module.exports = {
    ingreso,    
};