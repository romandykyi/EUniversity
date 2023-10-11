import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import Home from "./components/Home";
import Login from "./components/Pages/Login";
import ChangePassword from "./components/Pages/ChangePassword";
import RegisterNewUsers from "./components/Pages/RegisterNewUsers";

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
    path: '/register-users',
    requireAuth: true,
    element: <RegisterNewUsers/>,
    requireAdminRight: true,
  },
  {
    path: '/login',
    element: <Login/>   
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
