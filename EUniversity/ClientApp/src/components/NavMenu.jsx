import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import LoginMenu from "./api-authorization/LoginMenu.js";
import '../styles/NavMenu.css';
import authService from "./api-authorization/AuthorizeService";
import {ADMINISTRATOR_ROLE} from "./api-authorization/Roles";
import { useAppDispatch, useAppSelector } from '../store/store';
import { setIsAdmin } from '../store/features/isAdminSlice';


const NavMenu = () => {

  const [collapsed, setCollapsed] = useState(false);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const dispatch = useAppDispatch();
  const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

  const authNavList = [
        {
            to: "/change-password",
            title: "Change password"
        },
    ];

    const adminNavList = [
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
    ];

    const AuthNav = () => {
        return (
            <ul>
                <li onClick = {() => setCollapsed(false)} className="bg-sky-blue px-3 py-1 rounded-lg">
                    <Link className="text-white text-2xl font-medium w-full block"  to="/">Home</Link>
                </li>
                {
                    authNavList.map(link => <li key={link.title} onClick = {() => setCollapsed(false)}><Link className="text-black text-2xl font-medium px-3 py-1 w-full block" to={link.to}>{link.title}</Link></li>)
                }
            </ul>
        )
    };
    const AdminNav = () => {
        return (
            <ul className =" flex flex-col justify-end gap-3">
                <li onClick = {() => setCollapsed(false)} className="bg-sky-500 px-3 py-1 rounded-lg w-full inline">
                    <Link className="text-white text-2xl font-medium w-full block"  to="/">Home</Link>
                </li>
                {
                    adminNavList.map(link => <li key={link.title} onClick = {() => setCollapsed(false)}><Link className="text-black text-2xl font-medium px-3 py-1 w-full block" to={link.to}>{link.title}</Link></li>)
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

  return (
      <header className="py-3 shadow-md bg-white">
        <div className="container max-w-[1100px] flex items-center gap-4 justify-between">
          <Link className="text-2xl font-bold" to="/">EUniversity</Link>
            <nav className={`shadow-lg flex flex-col justify-center px-4 bg-white w-[300px] h-full fixed transition-all top-0 duration-300 ${collapsed ? "right-0" : "right-[-100%]"}`}>
                {
                isAuthenticated
                    ? isAdmin
                        ? <AdminNav />
                        : <AuthNav />
                    : ""
                }
            </nav>
            <div className="flex items-center gap-4">
            <LoginMenu />
            {
                isAuthenticated
                    ? <div className="flex items-center justify-between w-10 h-6 flex-col cursor-pointer z-20 " onClick={toggleNavbar}>
                        <div className="w-full h-1 bg-black rounded-full"></div>
                        <div className="w-full h-1 bg-sky-500 rounded-full"></div>
                        <div className="w-full h-1 bg-black rounded-full"></div>
                      </div>
                : ""
            }
          </div>
        </div>
      </header>
  );
}

export default NavMenu;
