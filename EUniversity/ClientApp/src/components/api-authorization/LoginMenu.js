import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import authService from './AuthorizeService';
import { ApplicationPaths } from './ApiAuthorizationConstants';

const LoginMenu = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [userName, setUserName] = useState(null);

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

    setIsAuthenticated(authStatus);
    setUserName(user && user.name);
  };

  const authenticatedView = (userName, profilePath, logoutPath, logoutState) => (
    <ul className="flex items-center gap-3">
      <li>
        <Link className="text-black font-medium" to={profilePath}>
          Hello {userName}
        </Link>
      </li>
      <li>
        <Link replace className="text-black font-medium" to={logoutPath} state={logoutState}>
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

  const { Register, Login, Profile, LogOut } = ApplicationPaths;

  return isAuthenticated
    ? authenticatedView(userName, Profile, LogOut, { local: true })
    : anonymousView(Register, Login);
};

export default LoginMenu;
