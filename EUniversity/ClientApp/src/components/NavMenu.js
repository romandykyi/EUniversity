import React, { useState, useEffect } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { LoginMenu } from './api-authorization/LoginMenu';
import './NavMenu.css';
import authService from "./api-authorization/AuthorizeService";
import {ADMINISTRATOR_ROLE} from "./api-authorization/Roles";

const AuthNav = () => {
    return (
        <div className="navbar-nav flex-grow">
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
            </NavItem>

            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/change-password">Change password</NavLink>
            </NavItem>
        </div>
    )
};
const AdminNav = () => {
    return (
        <div className="navbar-nav flex-grow">
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
            </NavItem>

            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/change-password">Change password</NavLink>
            </NavItem>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/students">Students</NavLink>
            </NavItem>
            <NavItem>
                <NavLink tag={Link} className="text-dark" to="/register-users">Register users</NavLink>
            </NavItem>
        </div>
    )
};
const NavMenu = () => {

  const [collapsed, setCollapsed] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);

    useEffect(() => {
        const checkAuthentication = async () => {
            const isAuthenticated = await authService.isAuthenticated();
            setIsAuthenticated(isAuthenticated);
            const isAdmin = await authService.isInRole(ADMINISTRATOR_ROLE);
            setIsAdmin(isAdmin);
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
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">EUniversity</NavbarBrand>
          <NavbarToggler onClick={toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
            <ul className="navbar-nav flex-grow">
                {
                    isAuthenticated
                    ? isAdmin
                        ? <AdminNav/>
                        :  <AuthNav/>
                    : ""
                }
              <LoginMenu />
            </ul>
          </Collapse>
        </Navbar>
      </header>
  );
}

export default NavMenu;
