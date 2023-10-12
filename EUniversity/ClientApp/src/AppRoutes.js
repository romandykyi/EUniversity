import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Home from "./components/Home";
import Login from "./components/Pages/Login";
import ChangePassword from "./components/Pages/ChangePassword";
import RegisterNewUsers from "./components/Pages/RegisterNewUsers";
import AdminStudents from "./components/Pages/AdminStudents";

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
    path: '/students',
    requireAuth: true,
    element: <AdminStudents/>,
    requireAdminRight: true,
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
