import { Router, Request, Response } from 'express';
import { CustomResponse } from '../../middlewares/interfaces/customResponse';


const router = Router();
const helper = require("./helper");

console.log("Inicializa users routes");

// Agregar un registro (POST /users)
/*
router.post('/', async (req: Request, res: Response) => {
  const { name, email } = req.body;
  try {
    const [result] = await pool.execute(
      'INSERT INTO users (name, email) VALUES (?, ?)',
      [name, email]
    );
    res.status(201).json({ id: (result as any).insertId, name, email });
  } catch (error) {
    res.status(500).json({ error: 'Error al agregar usuario' });
  }
});
*/
// Actualizar un registro (PUT /users/:id)
/*
router.put('/:id', async (req: Request, res: Response) => {
  const { id } = req.params;
  const { name, email } = req.body;
  try {
    await pool.execute(
      'UPDATE users SET name = ?, email = ? WHERE id = ?',
      [name, email, id]
    );
    res.json({ id, name, email });
  } catch (error) {
    res.status(500).json({ error: 'Error al actualizar usuario' });
  }
});
*/
// Eliminar un registro (DELETE /users/:id)
/*
router.delete('/:id', async (req: Request, res: Response) => {
  const { id } = req.params;
  try {
    await pool.execute('DELETE FROM users WHERE id = ?', [id]);
    res.json({ message: 'Usuario eliminado' });
  } catch (error) {
    res.status(500).json({ error: 'Error al eliminar usuario' });
  }
});
*/
// Opcional: Obtener todos los registros (GET /users)
router.get('/login',
  async (req: Request, res: Response) => {
    let status: number = 200;
    let customResponse: CustomResponse = {
      meta: {
        method: 'get',
        operation: '/login'
      },
      data: null,
    };

    try {
      console.log("get in login route");

      console.log(req.body);
      helper.ingreso(req.body)
        .then((data: any) => {
          customResponse.data = data;
          res.status(status).send(customResponse);
        })
        .catch((err: any) => {
          console.error("catch in login route");
          customResponse.error = err;
          status = 401;
          res.status(status).send(customResponse);
        });
    } catch (error: any) {
      customResponse.error = error;
      status = 500;
      res.status(status).send(customResponse);
    }
  });

export default router;
