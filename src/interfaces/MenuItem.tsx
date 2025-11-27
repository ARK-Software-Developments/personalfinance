// Interfaz para los elementos del menú
export interface MenuItem {
    id: number;
    nombre: string;
    nivel: number;
    parentId: number;
    accion?: string; // Enlace opcional para items sin submenú
    subMenu?: SubMenu[]; // Submenús opcionales
    titulo: string;
}

export interface SubMenu {
    id: number;
    nombre: string;
    nivel: number;
    accion?: string; // Enlace opcional para items sin submenú
    titulo: string;
}