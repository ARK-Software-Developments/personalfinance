import { MenuItem } from "./front/interfaces/MenuItem";
import { Menu } from "./front/Menu";
import db from "./data/database.json"

const menuItems: MenuItem[] = db.menues;

export function Content() {

  return (
    <div className="Content">
        <div>
          <Menu items={menuItems} />
          </div>
          <p></p>
          <div>
            Contenido
          </div>
    </div>
  );
};