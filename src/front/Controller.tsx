import { EntitiesController } from "./modules/EntitiesController";


export function Controller(document: any) {
  let content: any = "Contenido";
  let hrefValue = document.location.pathname;

  // Verificar si el elemento existe y capturar el href
  if (!hrefValue) {
    return 'No se encontró ningún enlace.';
  }

  // /cat/1
  if (hrefValue.indexOf("cat/1") == 1) {

    content = EntitiesController(document);

  };

  return content;
};



