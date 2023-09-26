import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import Home from "./components/Home";
import Login from "./components/Pages/Login";
import ChangePassword from "./components/Pages/ChangePassword";

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
    path: '/change-password',
    requireAuth: true,
    element: <ChangePassword />
  },
  {
    path: '/login',
    element: <Login/>   
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
