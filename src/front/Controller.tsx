import db from '../data/database.json';
import { Entidad } from './interfaces/Entidades';

export function Controller(document: any) {
    let content: any = "Contenido";
    let hrefValue = document.location.pathname;

    // Verificar si el elemento existe y capturar el href
    if (!hrefValue) {
        return 'No se encontró ningún enlace.';
    }

    // /cat/1
    if (hrefValue.indexOf("cat/1") == 1) {
        let data: Entidad[] = db.entidades;
        console.log(data)
        content =  DataTable(data);
    }

    return content;
};

const DataTable = (data: Entidad[]) => {
  return (
    <table border={1} style={{ width: '100%', borderCollapse: 'collapse' }}>
      <thead>
        <tr>
          <th>ID</th>
          <th>ENTIDAD</th>
          <th>TIPO</th>
        </tr>
      </thead>
      <tbody>
        {data.map((item: Entidad) => (
          <tr key={item.id}>
            <td>{item.id}</td>
            <td>{item.entity}</td>
            <td>{item.type}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};