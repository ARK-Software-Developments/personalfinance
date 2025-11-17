import React, { useState } from 'react';
import { MenuItem } from '../interfaces/MenuItem';

// Props del componente
interface HorizontalMenuProps {
  items: MenuItem[];
}

export const Menu: React.FC<HorizontalMenuProps> = ({ items }) => {
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
            key={item.label}
            className="menu-item"
            onMouseEnter={() => handleMouseEnter(item.label)}
            onMouseLeave={handleMouseLeave}
            title={item.title}
          >
            {item.href ? (
              <a href={item.href}>{item.label}</a>
            ) : (
              <span>{item.label}</span>
            )}
            {item.subItems && (
              <ul className={`submenu ${openSubmenu === item.label ? 'open' : ''}`}>
                {item.subItems.map((subItem) => (
                  <li key={subItem.label} className="submenu-item" title={subItem.title}>
                    <a href={subItem.href}>{subItem.label}</a>
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