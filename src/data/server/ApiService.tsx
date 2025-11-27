import { MenuItem } from "../../interfaces/MenuItem";


export async function ApiService(method: string = 'get', body: any = null, endpoint: string = '/'): Promise<any[]> {

  let urlApi = `${process.env.REACT_APP_PUBLIC_URL_API}:${process.env.REACT_APP_PORT_API}${process.env.REACT_APP_PUBLIC_URL_APIVERSION}${endpoint}`;
  // console.log("URL API:", urlApi);

  const response = await fetch(`${urlApi}`,
    {
      headers: {
        'Content-Type': 'application/json',
      },
      method: method,
    }
  );

  if (!response.ok) {
    throw new Error(`Error HTTP: ${response.status}`);
  }
  
  //const data: MenuItem[] = (await response.json()).data;

  return await response.json();
};