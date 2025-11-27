import { MenuItem } from "./interfaces/MenuItem";
import { Menu } from "./front/Menu";
import db from "./data/database.json"
import { Controller } from "./front/Controller";
import { ApiService } from "./data/server/ApiService";
import { useEffect, useState } from "react";
import { StorageSession } from "./data/server/Storage";

//const menuItems: MenuItem[] = db.menues;

export function Content() {

  const [menuItems, setData] = useState([]);

  useEffect(() => {

    if (StorageSession.existsItem('menuData')) {
      const storedData = JSON.parse(StorageSession.getItem('menuData')!);
      setData(storedData.data as any);
    }
    else {
      ApiService('get', null, '/menus/getall')
        .then((response: any) => {
          StorageSession.setItem('menuData', JSON.stringify(response));
          const data: MenuItem[] = response.data;
          //console.log("Response Data:", data);
          setData(data as any)

        })
    }
  }, []);

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