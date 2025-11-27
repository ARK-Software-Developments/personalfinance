import React, { useState } from 'react';
import { HorizontalMenuProps } from '../interfaces/HorizontalMenuProps';

export const Menu: React.FC<HorizontalMenuProps> = ({ items }) => {

  //console.log("Menu items:", items);
  const [openSubmenu, setOpenSubmenu] = useState<string | null>(null);

  const handleMouseEnter = (label: string) => {
    setOpenSubmenu(label);
  };

  const handleMouseLeave = () => {
    setOpenSubmenu(null);
  };

  const handleClick = (href?: string) => {
    if (href) {
      // onItemClick(href); // Captura el evento y pasa el href
    } else if (href) {
      window.location.href = href; // Fallback si no hay manejador
    }
  };

  return (
    <nav className="horizontal-menu">
      <ul className="menu-list">
        {items.map((item) => (
          <li
            key={item.nombre}
            className="menu-item"
            onMouseEnter={() => handleMouseEnter(item.nombre)}
            onMouseLeave={handleMouseLeave}
            title={item.titulo}
          >
            {item.accion ? (
              <a href={item.accion}>{item.nombre}</a>
            ) : (
              <span>{item.nombre}</span>
            )}
            {item.subMenu && (
              <ul className={`submenu ${openSubmenu === item.nombre ? 'open' : ''}`}>
                {item.subMenu.map((subItem) => (
                  <li key={subItem.nombre} className="submenu-item" title={subItem.titulo}>
                    <a href={subItem.accion}>{subItem.nombre}</a>
                  </li>
                ))}
              </ul>
            )}
          </li>
        ))}
      </ul>
    </nav>
  );
};