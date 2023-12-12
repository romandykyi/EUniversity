import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import LoginMenu from "./api-authorization/LoginMenu.js";
import authService from "./api-authorization/AuthorizeService.js";
import {ADMINISTRATOR_ROLE} from "./api-authorization/Roles.js";
import { useAppDispatch, useAppSelector } from '../store/store.js';
import { setIsAdmin } from '../store/features/isAdminSlice.js';
import { motion } from "framer-motion";
import { setIsThemeDarkRedux } from '../store/features/themeSlice.js';

const spring = {
    type: "spring",
    stiffness: 700,
    damping: 30
  };

const Header = () => {

  const [collapsed, setCollapsed] = useState(false);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isThemeDark, setIsThemeDark] = useState(false);
  const isThemeDarkRedux = useAppSelector(state => state.theme.isThemeDark);
  const dispatch = useAppDispatch();
  const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

  const authNavList = [
        {
            to: "/change-password",
            title: "Change password"
        },
        {
            to: "/groups",
            title: "Groups"
        },
        {
            to: "/courses",
            title: "Courses"
        },
        {
            to: "/classrooms",
            title: "Classrooms"
        },
        {
            to: "/semesters",
            title: "Semesters"
        },
        {
            to: "/classes",
            title: "Classes"
        },
    ];

    const adminNavList = [
        {
            to: "/change-password",
            title: "Change password"
        },
        {
            to: "/users",
            title: "Users"
        },
        {
            to: "/groups",
            title: "Groups"
        },
        {
            to: "/courses",
            title: "Courses"
        },
        {
            to: "/classrooms",
            title: "Classrooms"
        },
        {
            to: "/semesters",
            title: "Semesters"
        },
        {
            to: "/classes",
            title: "Classes"
        },
    ];

    const AuthNav = () => {
        return (
            <ul className =" flex flex-col justify-end gap-3">
                <li onClick = {() => setCollapsed(false)} className="bg-theme px-3 py-1 rounded-lg w-full inline">
                    <Link className="text-white text-2xl font-medium w-full block"  to="/">Home</Link>
                </li>
                {
                    authNavList.map(link => <li key={link.title} onClick = {() => setCollapsed(false)}><Link className="text-text text-2xl font-medium px-3 py-1 w-full block" to={link.to}>{link.title}</Link></li>)
                }
            </ul>
        )
    };
    const AdminNav = () => {
        return (
            <ul className =" flex flex-col justify-end gap-3">
                <li onClick = {() => setCollapsed(false)} className="bg-theme px-3 py-1 rounded-lg w-full inline">
                    <Link className="text-white text-2xl font-medium w-full block"  to="/">Home</Link>
                </li>
                {
                    adminNavList.map(link => <li key={link.title} onClick = {() => setCollapsed(false)}><Link className="text-text text-2xl font-medium px-3 py-1 w-full block" to={link.to}>{link.title}</Link></li>)
                }
            </ul>
        )
    };

    useEffect(() => {
        const checkAuthentication = async () => {
            const isAuthenticated = await authService.isAuthenticated();
            setIsAuthenticated(isAuthenticated);
            const isAdmin = await authService.isInRole(ADMINISTRATOR_ROLE);
            dispatch(setIsAdmin(isAdmin));
        };
        checkAuthentication();
        const subscriptionId = authService.subscribe(checkAuthentication);

        return () => {
            authService.unsubscribe(subscriptionId);
        };
    }, []);

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  };

  useEffect(() => {
    if (localStorage.theme === 'dark' || !('theme' in localStorage)) {
      document.documentElement.classList.add('dark')
      setIsThemeDark(true);
      dispatch(setIsThemeDarkRedux(true));
    } else {
      document.documentElement.classList.remove('dark')
      setIsThemeDark(false);
      dispatch(setIsThemeDarkRedux(false));
    }
  },[]);

  const changeTheme = () => {
    setIsThemeDark(prevState => {
      const newTheme = !prevState;
  
      if (newTheme) {
        document.documentElement.classList.add('dark');
        localStorage.setItem('theme', 'dark');
        dispatch(setIsThemeDarkRedux(true));
      } else {
        document.documentElement.classList.remove('dark');
        localStorage.setItem('theme', '');
        dispatch(setIsThemeDarkRedux(false));
      }
  
      return newTheme;
    });
  };

  return (
      <header className="py-3 shadow-md bg-background">
        <div className="container max-w-[1100px] flex items-center gap-4 justify-between">
          <Link className="text-2xl font-bold overflow-hidden whitespace-nowrap text-ellipsis" to="/">EUniversity</Link>
            <nav className={`z-20 shadow-lg flex flex-col justify-center px-4 bg-background w-[300px] h-full fixed transition-all top-0 duration-300 ${collapsed ? "right-0" : "right-[-100%]"}`}>
                {
                    isAuthenticated
                        ? isAdmin
                            ? <AdminNav />
                            : <AuthNav />
                        : ""
                }
            </nav>
            <div className="flex items-center gap-4">
                <div className={`bg-text flex rounded-full items-center p-2 cursor-pointer w-[60px] h-9 ${isThemeDark ? 'justify-end' : 'justify-start'}`} data-isOn={isThemeDark} onClick={changeTheme}>
                    <motion.div className="bg-background h-6 w-6 rounded-full" layout transition={spring} />
                </div>
                <LoginMenu />
                {
                    isAuthenticated
                        ? <div className="flex items-center justify-between w-10 h-6 flex-col cursor-pointer z-20 " onClick={toggleNavbar}>
                            <div className="w-full h-1 bg-text rounded-full"></div>
                            <div className="w-full h-1 bg-text rounded-full"></div>
                            <div className="w-full h-1 bg-text rounded-full"></div>
                        </div>
                    : ""
                }
          </div>
        </div>
      </header>
  );
}

export default Header;
