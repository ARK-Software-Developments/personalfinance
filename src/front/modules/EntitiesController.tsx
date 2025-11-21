import { Entidad } from "../../interfaces/Entidad";
import { GeneralResponse } from "../../interfaces/GeneralResponse";
import db from "../../data/database.json"
import { EF } from "../../data/server/EntidadesFinancieras";

const BASE_URL = `${process.env.REACT_APP_PUBLIC_URL}:${process.env.REACT_APP_PORT}/cat/1`;

const Input = (defaultValue: any, readOnly: boolean, label: string, id: string, hidden: boolean) => {
    if (readOnly) {
        return (<input type="text" name={label} id={id} className="form-input" required defaultValue={defaultValue} readOnly hidden={hidden} />);
    }
    else {
        return (<input type="text" name={label} id={id} className="form-input" required defaultValue={defaultValue} />);
    }
};

const Formulario = (data: Entidad, readOnly: boolean) => {

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault(); // Evita el envío predeterminado del formulario

        const form = event.target as HTMLFormElement;
        const formData = new FormData(form);

        // Accede a los valores de los campos del formulario
        const id = formData.get('id') as string;
        const entity = formData.get('entity') as string;
        const entitytype = formData.get('entitytype') as string;

        console.log('id:', id);
        console.log('entity:', entity);
        console.log('entitytype:', entitytype);

        window.location.href = BASE_URL;
    };

    return (
        <div className="main-container">
            <div className="form-wrapper">
                <form
                    onSubmit={handleSubmit}
                    method="POST"
                    encType="multipart/form-data">

                    <div className="form-header">
                        <h2 className="form-title">Entidad Financiera</h2>
                        <p className="form-description">
                            Please fill out the form below to confirm your sponsorship
                            for the event. All required fields are marked with *.
                        </p>
                    </div>

                    <div className="form-group">
                        <label htmlFor="entity" className="form-label">Organization / Company Name*</label>
                        {Input(data.entity, readOnly, "entity", data.entity, false)}
                    </div>

                    <div className="form-group">
                        <label htmlFor="entitytype" className="form-label">Organization Type</label>
                        {Input(data.entitytype, readOnly, "entitytype", data.entitytype, false)}
                    </div>
                    <div className="form-group">
                        {Input(data.id, true, "id", data.id.toString(), true)}
                    </div>
                    <button type="submit" className="submit-btn">Confirm</button>
                </form>
            </div>
        </div>
    );
};

const DataTable = (data: Entidad[]) => {
    return (
        <table className="grid-table">
            <thead>
                <tr>
                    <th className="buttons-cell"></th>
                    <th className="cell-center">ID</th>
                    <th className="cell-left">ENTIDAD</th>
                    <th className="cell-center">TIPO</th>
                </tr>
            </thead>
            <tbody>
                {data.map((item: Entidad) => (
                    <tr key={item.id}>
                        <td className="buttons-cell" id={item.id.toString()}>
                            <button className="btn" onClick={handleClick}>Ver</button>
                            <button className="btn" onClick={handleClick}>Editar</button>
                            <button className="btn" onClick={handleClick}>Eliminar</button>
                        </td>
                        <td className="cell-center">{item.id}</td>
                        <td className="cell-left">{item.entity}</td>
                        <td className="cell-center">{item.entitytype}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export function EntitiesController(document: any) {

    let content: any = "No hay contenido para mostrar.";

    let dbMysqlConn = new EF().GetAll();

    let generalResponse: GeneralResponse = db.entidades[0];

    console.log("Event:", generalResponse);

    if (document.location.pathname.indexOf("/entity/") > 0) {        
        let data = generalResponse.data as Entidad[];
        let id = parseInt(document.location.pathname.split("/")[5]);
        let readOnly = document.location.pathname.indexOf("/update/") > 0 ? false : true;
        content = Formulario(data.find(item => item.id === id) as Entidad, readOnly);
    }
    else {
        content = DataTable(generalResponse.data as Entidad[]);
    }

    return content;
};

const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    console.log('¡Se hizo clic en el botón!');
    let operacion = null;
    switch (event.currentTarget.innerHTML) {
        case 'Ver':
            operacion = `/entity/view/${event.currentTarget.parentElement?.id}`;
            break;

        case 'Editar':
            operacion = `/entity/update/${event.currentTarget.parentElement?.id}`;
            break;

        case 'Eliminar':
            operacion = `/entity/delete/${event.currentTarget.parentElement?.id}`;
            break;

        default:
            break;
    }

    window.location.href = `${BASE_URL}${operacion}`;

    // Puedes acceder a propiedades del evento si es necesario
    // event.preventDefault();
};