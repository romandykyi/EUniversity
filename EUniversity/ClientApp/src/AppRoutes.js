import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Home from "./components/Pages/Home";
import Login from "./components/Pages/Login";
import ChangePassword from "./components/Pages/ChangePassword";
import RegisterNewUsers from "./components/Pages/RegisterNewUsers";
import AdminUsers from "./components/Pages/AdminUsers";
import AdminGroups from './components/Pages/AdminGroups';
import AdminGroup from "./components/Pages/AdminGroup";
import AdminCourses from "./components/Pages/AdminCourses";
import AdminClassrooms from './components/Pages/AdminClassrooms';
import AdminClassroom from './components/Pages/AdminClassroom';

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
  },
  {
    path: '/groups',
    requireAuth: true,
    element: <AdminGroups/>,
  },
  {
    path: '/groups/:id',
    requireAuth: true,
    element: <AdminGroup/>,
  },
  {
    path: '/courses',
    requireAuth: true,
    element: <AdminCourses/>,
  },
  {
    path: '/classrooms',
    requireAuth: true,
    element: <AdminClassrooms/>,
  },
  {
    path: '/classrooms/:id',
    requireAuth: true,
    element: <AdminClassroom/>,
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
