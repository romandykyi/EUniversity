import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import authService from './AuthorizeService';
import logoutImg from '../../assets/logout.svg';
import profileImg from '../../assets/profile.svg';
import logoutWhiteImg from '../../assets/logoutWhite.svg';
import profileWhiteImg from '../../assets/profileWhite.svg';
import { ApplicationPaths } from './ApiAuthorizationConstants';
import { useAppSelector } from '../../store/store';

const LoginMenu = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [fullName, setFullName] = useState('');
  const isThemeDark = useAppSelector((state) => state.theme.isThemeDark);

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
      authService.getUser(),
    ]);
    if (user) {
      setFullName(`${user.family_name} ${user.given_name}`);
    }

    setIsAuthenticated(authStatus);
  };

  const authenticatedView = (userName, logoutPath, logoutState) => (
    <ul className='flex items-center gap-4'>
      <li className='w-8'>
        <Link
          className='text-text font-medium relative z-40 lg:z-0 w-8'
          to='/profile'
        >
          <img
            className='w-full max-w-8'
            src={isThemeDark ? profileWhiteImg : profileImg}
            alt='profile'
          />
        </Link>
      </li>
      <li className='w-8'>
        <Link
          replace
          className='text-text font-medium relative z-40 lg:z-0 w-8'
          to={logoutPath}
          state={logoutState}
        >
          <img
            className='w-full max-w-8'
            src={isThemeDark ? logoutWhiteImg : logoutImg}
            alt='logout'
          />
        </Link>
      </li>
    </ul>
  );

  const anonymousView = (registerPath) => (
    <ul>
      <li>
        <Link className='text-text font-medium' to='/login'>
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
