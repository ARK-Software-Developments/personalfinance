import { Helmet } from "react-helmet";

export function Header() {

  return (
    <div className="App-Header">
        <Helmet>
        <title>{process.env.REACT_APP_TITLE}</title>
        {/* Puedes agregar más metaetiquetas aquí */}
        <meta name="description" content="Esta página tiene un título dinámico." />
      </Helmet>
      <h1>Bienvenido a {process.env.REACT_APP_TITLE}</h1>
    </div>
  );
};