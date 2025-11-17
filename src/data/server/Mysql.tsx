import mysql from 'mysql2/promise';

export function CONN() {
  try {
    // Crear conexión
    const connection = mysql.createPool({
      host: process.env.REACT_APP_DBSERVER,      // Cambia por tu host
      user: process.env.REACT_APP_DBUSER,     // Cambia por tu usuario
      password: process.env.REACT_APP_DBPASS, // Cambia por tu contraseña
      database: process.env.REACT_APP_DBNAME, // Cambia por el nombre de tu BD
      waitForConnections: true,
      connectionLimit: 10,
      queueLimit: 0
    });

    console.log('Conexión exitosa a MySQL');

    return connection;

    // Cerrar conexión
    // await connection.end();
  } catch (error) {
    console.error('Error:', error);
    throw error;
  }
};


// Función asíncrona para conectar y consultar
async function conectarYConsultar(conn: any) {
  try {
    // Crear conexión
    const connection = await mysql.createConnection({
      host: process.env.REACT_APP_DBSERVER,      // Cambia por tu host
      user: process.env.REACT_APP_DBUSER,     // Cambia por tu usuario
      password: process.env.REACT_APP_DBPASS, // Cambia por tu contraseña
      database: process.env.REACT_APP_DBNAME// Cambia por el nombre de tu BD
    });

    console.log('Conexión exitosa a MySQL');

    // Ejecutar consulta (ejemplo: obtener todos los usuarios de una tabla)
    const [rows] = await conn.execute("SELECT `id`,`entity`,`type` FROM `entities`;");

    // Mostrar resultados
    console.log('Datos obtenidos:', rows);

    // Cerrar conexión
    await connection.end();
  } catch (error) {
    console.error('Error:', error);
  }
}

// Llamar a la función conectarYConsultar();