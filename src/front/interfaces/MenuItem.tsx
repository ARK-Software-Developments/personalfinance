// Interfaz para los elementos del menú
export interface MenuItem {
    label: string;
    href?: string; // Enlace opcional para items sin submenú
    subItems?: MenuItem[]; // Submenús opcionales
    title: string;
}