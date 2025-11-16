import { MenuItem } from "./front/interfaces/MenuItem";
import { Menu } from "./front/Menu";
import db from "./data/database.json"
import { Controller } from "./front/Controller";

const menuItems: MenuItem[] = db.menues;

export function Content() {

  
  return (
    <div className="Content">
      <div>
        <Menu items={menuItems} />
      </div>
      <p></p>
      <div>
        {Controller(document)}
      </div>
    </div>
  );
};