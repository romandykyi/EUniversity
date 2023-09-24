import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Login from "./components/Pages/Login";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    requireAuth: true,
    element: <FetchData />
  },
  {
    path: '/login',
    element: <Login/>   
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
