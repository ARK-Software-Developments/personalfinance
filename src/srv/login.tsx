import { useState } from 'react'
import db from '../data/database.json';
import { Usuario } from '../data/usuario';

const usuarios: Usuario[] = db.usuarios;
var usuario: Usuario;

export const Login = () => {
    const [errorMessages, setErrorMessages] = useState({});
    const [isSubmitted, setIsSubmitted] = useState(false);

    // Generate JSX code for error message
    const renderErrorMessage = (name: string) =>
        name === errorMessages && (
            <div className="error">{errorMessages}</div>
        );

    const handleSubmit = (event: { preventDefault: () => void; }) => {
        //Prevent page reload
        event.preventDefault();

        var { uname, pass } = document.forms[0];

        let result = usuarios.find(x => x.email.toString() === uname.value.toString() && x.clave.toString() === pass.value.toString());
        if (result) {
            console.log("ok");
            usuario = result as Usuario;
        } else {
            console.error("error");
        }

    };

    // JSX code for login form
    const renderForm = (
        <div className="form">
            <form onSubmit={handleSubmit}>
                <div className="input-container">
                    <label>USUARIO: </label>
                    <input type="text" name="uname" required />
                    {renderErrorMessage("uname")}
                </div>
                <div className="input-container">
                    <label>CLAVE: </label>
                    <input type="password" name="pass" required />
                    {renderErrorMessage("pass")}
                </div>
                <div className="input-container">
                    {renderErrorMessage("failud")}
                </div>
                <div className="button-container">
                    <input type="submit" />
                </div>
            </form>
        </div>
    );

    return (
        <div className="app">
            <div className="login-form">
                <div className="title">INGRESO A LA APP</div>
                <div>
                    <h2>Bienvenido, {usuario.nombre}</h2>
                    <h3>Ingresos</h3>
                </div>
            </div>
        </div>
    );
};