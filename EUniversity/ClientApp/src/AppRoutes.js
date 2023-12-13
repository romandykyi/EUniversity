import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Home from "./components/Pages/Home";
import Login from "./components/Pages/Login";
import ChangePassword from "./components/Pages/ChangePassword";
import AdminUsers from "./components/Pages/AdminUsers";
import AdminGroups from './components/Pages/AdminGroups';
import AdminGroup from "./components/Pages/AdminGroup";
import AdminCourses from "./components/Pages/AdminCourses";
import AdminClassrooms from './components/Pages/AdminClassrooms';
import AdminClassroom from './components/Pages/AdminClassroom';
import ProfilePage from './components/Pages/ProfilePage';
import AdminSemesters from './components/Pages/AdminSemesters';
import AdminSemester from "./components/Pages/AdminSemester";
import AdminClasses from './components/Pages/AdminClasses';
import AdminClass from './components/Pages/AdminClass';

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
  {
    path: '/semesters',
    requireAuth: true,
    element: <AdminSemesters/>,
  },
  {
    path: '/semesters/:id',
    requireAuth: true,
    element: <AdminSemester/>,
  },
  {
    path: '/profile',
    requireAuth: true,
    element: <ProfilePage/>,
  },
  {
    path: '/classes',
    requireAuth: true,
    element: <AdminClasses/>,
  },
  {
    path: '/classes/:id',
    requireAuth: true,
    element: <AdminClass/>,
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
