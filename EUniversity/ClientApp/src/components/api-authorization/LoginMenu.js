import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import authService from './AuthorizeService';
import { ApplicationPaths } from './ApiAuthorizationConstants';

const LoginMenu = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [fullName, setFullName] = useState("");

  useEffect(() => {
    const subscription = authService.subscribe(() => populateState());
    populateState();

    return () => {
      authService.unsubscribe(subscription);
    };
  }, []);

  const populateState = async () => {
    const [authStatus, user] = await Promise.all([
      authService.isAuthenticated(),
      authService.getUser()
    ]);
    console.log(`user: ${user}`);
    if (user) {
      setFullName(`${user.family_name} ${user.given_name}`);
    }

    setIsAuthenticated(authStatus);
  };

  const authenticatedView = (userName, logoutPath, logoutState) => (
    <ul className="flex items-center gap-3">
      <li>
        <Link className="text-black font-medium relative z-40 lg:z-0" to="/profile">
          {userName}
        </Link>
      </li>
      <li>
        <Link replace className="text-black font-medium relative z-40 lg:z-0" to={logoutPath} state={logoutState}>
          Logout
        </Link>
      </li>
    </ul>
  );

  const anonymousView = (registerPath) => (
    <ul>
      <li>
        <Link className="text-black font-medium" to="/login">
          Login
        </Link>
      </li>
    </ul>
  );

  const { Register, Login, LogOut } = ApplicationPaths;

  return isAuthenticated
    ? authenticatedView(fullName, LogOut, { local: true })
    : anonymousView(Register, Login);
};

export default LoginMenu;
