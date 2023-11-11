import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Home from "./components/Home";
import Login from "./components/Pages/Login";
import ChangePassword from "./components/Pages/ChangePassword";
import RegisterNewUsers from "./components/Pages/RegisterNewUsers";
import AdminUsers from "./components/Pages/AdminUsers";
import AdminGroups from './components/Pages/AdminGroups';
import AdminGroup from "./components/Pages/AdminGroup";

const AppRoutes = [
  {
    index: true,
    element: <Home />
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
  {
    path: '/users',
    requireAuth: true,
    element: <AdminUsers/>,
    requireAdminRight: true,
  },
  {
    path: '/groups',
    requireAuth: true,
    element: <AdminGroups/>,
    requireAdminRight: true,
  },
  {
    path: '/groups/:id',
    requireAuth: true,
    element: <AdminGroup/>,
    requireAdminRight: true,
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
